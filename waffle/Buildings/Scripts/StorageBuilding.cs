using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StorageBuilding : Building
{
    [Export] public int maxCapacity {get;set;}
    private JobManager jobManager;
    public Node2D entrance {get;set;}


    public override void _Ready()
    {
        base._Ready();
        jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        entrance = GetNode<Node2D>("Entrance");

        maxCapacity = 100;

        //AddResourceToInventory(Colony.ResourceType.Wood, 10);
        //AddResourceToInventory(Colony.ResourceType.Stone, 10);
    }

    public bool IsFull()
    {
        int sum = 0;
        foreach(int val in storageDict.Values)
        {
            sum += val;
        }

        if (sum >= maxCapacity)
        {
            return true;
        }
        return false;
    }

    public Drop CreateDrop(Colony.ResourceType type)
    {
        Drop createdDrop = Helper.CreateDrop(GlobalPosition,type,GetTree().GetFirstNodeInGroup("WORLD"), false);
        createdDrop.forStorageRetrieval = true;
        RemoveResourceFromInventory(type, 1);
        
        return createdDrop;
    }
    public override void AddResourceToInventory(Colony.ResourceType type, int amt)
    {
        base.AddResourceToInventory(type, amt);

        // check requests
        foreach (Request r in jobManager.RequestList.ToList())
        {
            if (r.resourceType == type)
            {
                r.FullfillRequest(CreateDrop(type));
            }
        }
    }


    public void CreateTask(Colony.ResourceType type, Drop drop = null)
    {
        FetchTask newTask = new FetchTask(this, type, drop);
        jobManager.AddTask(Colony.Job.Laborer,newTask);
    }

}
