using Godot;
using System;
using System.Collections.Generic;

public partial class Harvestable: ProductionObject
{

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
    }

    private void MatchType()
    {
        switch (type)
        {
            case Colony.Type.Wood:
                break;
            case Colony.Type.Stone:
                break;
        }
    }

    public override void Produce()
    {
        // create drops
        GD.Print("ok");

        if (type == Colony.Type.Berries) return;

        Remove();
    }

    public void CreateTask()
    {
        Task newTask = new ProduceTask(this, Task.Type.PRODUCE);
        jobManager.TaskDict.GetValueOrDefault(jobType).Add(newTask);

    }

    public void Remove()
    {
        ObjectMap.Remove(this.tile);
        QueueFree();
    }

}
