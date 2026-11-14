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
    [Export] public Drop drop {get;set;}
    public bool resourceRetrieved {get;set;}
    

    public FetchTask(StaticBody2D source, Colony.ResourceType type, Drop drop = null) : base(source)
    {
        resourceRetrieved = false;
        resourceType = type;
        // find resource
        this.drop = drop;
        
        if (this.drop == null)
        {
            failed = true;
            return;
        }
        targetPos = this.drop.GlobalPosition;
    }

    public void OnReosurceRetrieved()
    {
        resourceRetrieved = true;
        targetPos = sourceObject.GlobalPosition; 
        worker.drop = this.drop;
        this.drop.Visible = true;
    }

    private Node2D FindResource(Colony.ResourceType type)
    {
        List<Node> drops = sourceObject.GetTree().GetNodesInGroup("DROP").ToList(); // use something in the tree
        drops = drops.Where(d => !((Drop) d).forStorageRetrieval).ToList();
        Drop drop = (Drop) Helper.FindClosestNode(sourceObject.GlobalPosition,drops);

        if (drop != null) {
            drop.taskCreatedForDrop = true;
            return drop;
        } else
        {
            // find through stroage buildings
            List<Node> storageBuildings = sourceObject.GetTree().GetNodesInGroup("STORAGEBUILDING").ToList();
            storageBuildings = storageBuildings.Where(b => ((StorageBuilding) b).HasResource(type)).ToList();
            StorageBuilding closestStorage = (StorageBuilding) Helper.FindClosestNode(sourceObject.GlobalPosition,storageBuildings);

            // create drop at storage
            if (closestStorage != null)
            {
                drop = closestStorage.CreateDrop(type);
                drop.taskCreatedForDrop = true;
                drop.Visible = false;
                return drop;
            }
        }

        failed = true;
        return null;
    }

    public void DeliverResource()
    {
        worker.currentTask = null;
        worker.drop = null;
        drop.QueueFree();
        Building source = sourceObject as Building;
        source.AddResourceToInventory(resourceType,1);
    }
}