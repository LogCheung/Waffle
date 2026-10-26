using Godot;
using System;

public partial class ProduceTask : Task
{
    public float progress {get;set;}
    // Source building needs labor to produce good
    // Once task is completed, source building will produce respective good
    public ProduceTask(ProductionObject source) : base(source)
    {
        targetPos = source.GlobalPosition;
    }

    public void AddProgress(float amt)
    {
        progress += amt;

        if (progress >= 100) {
            worker.currentTask = null;
            sourceObject.Produce();
        }
    }
}
