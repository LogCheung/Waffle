using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FetchTask : Task
{
    // source building wants this resource
    // target will be storage building pos or ground pos (if harvested)
    // source & target can be same if it is a storage building wanting to take in resources
    // task will also handle returning resource to source building
    [Export] public Colony.ResourceType resourceType {get;set;}
    public bool resourceRetrieved {get;set;}
    

    public FetchTask(ProductionObject source, Colony.ResourceType type, Drop drop = null) : base(source)
    {
        resourceRetrieved = false;
        // find resource
        if (drop == null) targetPos = FindResource(type).GlobalPosition;
        else targetPos = drop.GlobalPosition;
    }

    public void OnReosurceRetrieved()
    {
        resourceRetrieved = true;
        targetPos = sourceObject.GlobalPosition; 
    }

    private Node2D FindResource(Colony.ResourceType type)
    {
        List<Node> drops = GetTree().GetNodesInGroup("DROP").ToList();
        Drop drop = (Drop) Helper.FindClosestNode(sourceObject.GlobalPosition,drops);

        if (drop != null) drop.taskCreatedForDrop = true;

        return drop;
    }

    public void DeliverResource()
    {
        worker.currentTask = null;
        Building source = sourceObject as Building;
        source.AddResourceToInventory(resourceType,1);
        GD.Print("delivered");
    }
}