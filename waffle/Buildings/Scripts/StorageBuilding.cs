using Godot;
using System;
using System.Collections.Generic;

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
        foreach (Colony.ResourceType type in Enum.GetValues(typeof(Colony.ResourceType)))
        {
            storageDict.Add(type, 0);
        }
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

    public void CreateTask(Colony.ResourceType type, Drop drop = null)
    {
        FetchTask newTask = new FetchTask(this, type, drop);
        jobManager.AddTask(Colony.Job.Laborer,newTask);
    }

}
