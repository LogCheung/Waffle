using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Workplace : Building, ProductionObject
{
    public enum State
    {
        Loading,
        Producing,
        Emptying,
    }

    [Export] public Recipe recipe {get;set;}
    [Export] public float progress {get;set;}
    [Export] public int maxProductCount {get;set;} = 5;
    [Export] public int maxInputCount {get;set;} = 5;
    [Export] public int inputThreshold {get;set;} = 3;
    [Export] public int maxWorkers {get;set;} = 4;

    private bool increasing;
    private State state;
    public override void _Ready()
    {
        base._Ready();

        GetNode<Label>("Label").Text = buildType.ToString();

        jobType = BuildInfo.buildJobs[buildType];
        recipe = BuildInfo.produceRecipes[buildType].First();

        UpdateState(true);
    }

    public void Produce()
    {
        // create drops (invisible drops)
        List<Colony.ResourceType> rewardTypes = recipe.rewards.Keys.ToList();
        RemoveRecipeInputsFromInventory(recipe);
        
        foreach (Colony.ResourceType r in rewardTypes)
        {
            for (int i = 0; i < recipe.rewards[r]; i++)
            {
                AddResourceToInventory(r,1);
            }
        }
        
        // recurring task

    }
    public override void AddResourceToInventory(Colony.ResourceType type, int amt)
    {
        base.AddResourceToInventory(type, amt);
        
        UpdateState();
    }

    public override void RemoveResourceFromInventory(Colony.ResourceType type, int amt)
    {
        base.RemoveResourceFromInventory(type, amt);
        UpdateState();
    }



    private void CreateDemand(int amt = -1)
    {
        int taskCount = amt == -1 ? maxInputCount : amt;
        List<Colony.ResourceType> inputTypes = recipe.inputs.Keys.ToList();
        foreach (Colony.ResourceType r in inputTypes)
        {
            for (int i = 0; i < taskCount;i++)
            {
                CreateFetchTask(r);
            }
        }
    }

    private bool IsThresholdReached()
    {
        bool thresholdReached = true;

        List<Colony.ResourceType> inputTypes = recipe.inputs.Keys.ToList();
        foreach (Colony.ResourceType r in inputTypes)
        {
            thresholdReached = storageDict[r] >= inputThreshold;

            if (!thresholdReached)
            {
                return thresholdReached;
            }
        }

        return thresholdReached;
    }

    private bool IsFull()
    {
        bool maxReached = true;

        List<Colony.ResourceType> rewardTypes = recipe.rewards.Keys.ToList();
        foreach (Colony.ResourceType r in rewardTypes)
        {
            maxReached = storageDict[r] >= maxProductCount;

            if (!maxReached)
            {
                return maxReached;
            }
        }

        return maxReached;
    }

    private void UpdateState(bool initialRun = false)
    {
        List<Colony.ResourceType> inputTypes = recipe.inputs.Keys.ToList();
        State newState; // random state
        if(IsFull())
        {
            newState = State.Emptying;
        } else if (!IsThresholdReached())
        {
            newState = State.Loading;
        } else // threshold reached
        {
            newState = State.Producing;
        }

        if (state == newState && !initialRun) return;

        state = newState;
        MatchState();

    }

    private void MatchState()
    {
        GD.Print(state);
        switch (state)
        {
            case (State.Loading):
                CreateDemand(inputThreshold);
                break;
            case (State.Producing):
                for(int i = 0; i < inputThreshold;i++) CreateProduceTask(jobType);
                break;
            case (State.Emptying):
                List<Colony.ResourceType> rewardTypes = recipe.rewards.Keys.ToList();
                List<Node> nodes = GetTree().GetNodesInGroup("BUILDING").Where(x => x is StorageBuilding).ToList();
                Node node = Helper.FindClosestNode(GlobalPosition, nodes);
                StorageBuilding storage = node as StorageBuilding;
                foreach (Colony.ResourceType r in rewardTypes)
                {
                    GD.Print(r.ToString() + "rewards: " + storageDict[r] );
                    for(int i = 0;i < storageDict[r]; i++)
                    {
                        CreateDeliverTask(jobType,storage,r);
                        RemoveResourceFromInventory(r,1);
                    }
                }
                break;
        }
    }



    public Task GetTask() // for citizens
    {
        if (outgoingTasks.Count == 0) return null;

        Task task = outgoingTasks.First();
        outgoingTasks.Remove(task);
        return task;
    }

    public void AssignWorker()
    {
        
    }


}
