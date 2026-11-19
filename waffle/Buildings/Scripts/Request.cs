using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public partial class Request
{
    public Colony.ResourceType resourceType {get;set;}
    public int Priority;
    public bool paused = false;
    public Building sourceObject {get;set;}
    private Colony.Job jobType {get;set;}
    private JobManager jobManager {get;set;}
    public static int huh = 0;

    public Request(Colony.ResourceType type, Building source, Colony.Job jobType)
    {
        this.resourceType = type;
        sourceObject = source;
        this.jobType = jobType;
        this.jobManager = (JobManager) sourceObject.GetTree().GetFirstNodeInGroup("JOBMANAGER");


        // inital check for resource, otherwise it will be fulfilled later
        FindResource();
    }

    private void FindResource()
    {
        huh++;
        // find through stroage buildings, filter storages to those with resource
        List<Node> storageBuildings = sourceObject.GetTree().GetNodesInGroup("STORAGEBUILDING").ToList();
        storageBuildings = storageBuildings.Where(b => ((StorageBuilding) b).HasResource(resourceType)).ToList();
        StorageBuilding closestStorage = (StorageBuilding) Helper.FindClosestNode(sourceObject.GlobalPosition,storageBuildings);

        // create drop at storage
        if (closestStorage != null)
        {
            Drop drop = closestStorage.CreateDrop(resourceType);
            drop.Visible = false;

            // Create fetch task for laborer

            //GD.Print($"Request for {resourceType} fulfilled");
            return;
        }
        return;
    }

    public void FullfillRequest(Drop drop)
    {
        // verify task has not been created for drop
        // Create fetch task for laborer
        FetchTask newTask = new FetchTask(sourceObject, resourceType,drop);
        jobManager.AddTask(jobType,newTask);
        drop.taskCreatedForDrop = true;
        JobManager.RequestList.Remove(this);
        GD.Print($"Request for {resourceType} fulfilled");

    }

}
