using Godot;
using System;
using System.Collections.Generic;

public partial class Harvestable: ProductionObject
{
    [Export] public int rewardAmount {get;set;}
    public static Dictionary<Vector2I,Harvestable> ObjectMap = new Dictionary<Vector2I, Harvestable>();
    private Sprite2D sprite;
    private JobManager jobManager;

    public override void _Ready()
    {
        jobType = Colony.Job.Laborer;

        ObjectMap.Add(this.tile, this);
        sprite = GetNode<Sprite2D>("Sprite2D");
        jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        sprite.Frame = (int) type;

        MatchType();
    }

    private void MatchType()
    {
        switch (type)
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

    public override void Produce()
    {
        // create drops
        World world = (World) GetTree().GetFirstNodeInGroup("WORLD");
        for (int i = 0; i < rewardAmount; i++)
        {
            PackedScene scene = GD.Load<PackedScene>("res://Resources/Scenes/drop.tscn");
            Drop drop = (Drop) scene.Instantiate();
            
            drop.GlobalPosition = GlobalPosition;
            drop.type = this.type;
            world.AddChild(drop);
        }

        if (type == Colony.ResourceType.Berries) return;
        Remove();
    }

    public void CreateTask()
    {
        Task newTask = new ProduceTask(this);
        jobManager.TaskDict.GetValueOrDefault(jobType).Add(newTask);

    }

    public void Remove()
    {
        ObjectMap.Remove(this.tile);
        QueueFree();
    }

}
