using Godot;
using System;

public partial class ProduceTask : Task
{
    public float progress {get;set;}
    // Source building needs labor to produce good
    // Once task is completed, source building will produce respective good
    public ProduceTask(StaticBody2D source) : base(source)
    {
        targetPos = source.GlobalPosition;
    }

    public void AddProgress(float amt)
    {
        progress += amt;

        if (progress >= 100) {
            worker.currentTask = null;

            // source must be a task object to produce something and to make this task
            TaskObject source = (TaskObject) sourceObject;
            source.Produce();
        }
    }
}
