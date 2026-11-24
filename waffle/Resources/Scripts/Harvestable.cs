using Godot;
using System;
using System.Collections.Generic;

public partial class Harvestable: StaticBody2D, ProductionObject
{
    [Export] public Colony.ResourceType productionType {get;set;} 
    [Export] public Colony.Job jobType {get;set;}
    [Export] public float progress {get;set;}
    [Export] public int rewardAmount {get;set;}

    public List<Vector2I> tiles {get;set;} 
    public static Dictionary<Vector2I,Harvestable> ObjectMap = new Dictionary<Vector2I, Harvestable>();
    private Sprite2D sprite;
    private JobManager jobManager;

    public override void _Ready()
    {
        jobType = Colony.Job.Laborer;

        foreach(Vector2I t in tiles) ObjectMap.Add(t, this);
        sprite = GetNode<Sprite2D>("Sprite2D");
        jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        sprite.Frame = (int) productionType ;

        MatchType();
    }

    private void MatchType()
    {
        switch (productionType)
        {
            case Colony.ResourceType.Wood:
                rewardAmount = GD.RandRange(3,5);
                break;
            case Colony.ResourceType.Stone:
                rewardAmount = GD.RandRange(3,5);
                break;
            case Colony.ResourceType.Berries:
                rewardAmount = GD.RandRange(2,4);
                break;
                
        }
    }

    public void Produce()
    {
        // create drops
        World world = (World) GetTree().GetFirstNodeInGroup("WORLD");
        for (int i = 0; i < rewardAmount; i++)
        {
            Helper.CreateDrop(GlobalPosition,this.productionType, world);
        }

        if (productionType == Colony.ResourceType.Berries) return;
        Remove();
    }

    public virtual void CreateTask()
    {
        Task newTask = new ProduceTask(this);
        jobManager.TaskDict.GetValueOrDefault(jobType).Add(newTask);

    }

    public void Remove()
    {
        foreach(Vector2I t in tiles) ObjectMap.Remove(t);
        QueueFree();
    }

}
