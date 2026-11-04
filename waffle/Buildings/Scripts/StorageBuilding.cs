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

        AddResourceToInventory(Colony.ResourceType.Wood, 10);
        AddResourceToInventory(Colony.ResourceType.Stone, 10);
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
        PackedScene scene = GD.Load<PackedScene>("res://Resources/Scenes/drop.tscn");
        Drop drop = (Drop) scene.Instantiate();
        RemoveResourceFromInventory(type, 1);
        
        drop.GlobalPosition = GlobalPosition;
        drop.type = type;
        drop.forStorageRetrieval = true;
        GetTree().GetFirstNodeInGroup("WORLD").AddChild(drop);
        GD.Print("created");
        return drop;
    }

    public void CreateTask(Colony.ResourceType type, Drop drop = null)
    {
        FetchTask newTask = new FetchTask(this, type, drop);
        jobManager.AddTask(Colony.Job.Laborer,newTask);
    }

}
