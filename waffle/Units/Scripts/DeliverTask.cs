using Godot;
using System;

public partial class DeliverTask : Task
{
    [Export] public Drop drop {get;set;}
    [Export] public Colony.ResourceType resourceType {get;set;}
    private Building receiverObject;
    public bool resourceRetrieved {get;set;}
    public DeliverTask(Building source, Building receiver, Colony.ResourceType resourceType) : base(source)
    {
        resourceRetrieved = false;
        sourceObject = source;
        receiverObject = receiver;
        this.resourceType = resourceType;
        targetPos = source.GlobalPosition;
    }
    public void OnSourceReached()
    {
        resourceRetrieved = true;
        targetPos = receiverObject.GlobalPosition;


        drop = Helper.CreateDrop(sourceObject.GlobalPosition,resourceType,sourceObject.GetParent(),true,true);
        worker.drop = this.drop;
        this.drop.Visible = true;
    }
    public void DeliverResource()
    {
        worker.currentTask = null;
        worker.drop = null;
        drop.QueueFree();
        receiverObject.AddResourceToInventory(resourceType,1);
    }
}
