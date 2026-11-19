using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract partial class Building : StaticBody2D, TaskObject
{
    [Export] public Colony.Job jobType {get;set;}
    [Export] public Colony.BuildType buildType {get;set;}
    [Export] public Vector2I tile {get;set;}
    [Export] public bool paused {get;set;}
    public static Dictionary<Vector2I,Building> ObjectMap = new Dictionary<Vector2I, Building>();
    protected Dictionary<Colony.ResourceType,int> storageDict = new Dictionary<Colony.ResourceType, int>();
    protected List<Request> outgoingRequests {get;set;} = new List<Request>();
    protected List<Task> outgoingTasks {get;set;} = new List<Task>();

    public override void _Ready()
    {
        foreach (Colony.ResourceType type in Enum.GetValues(typeof(Colony.ResourceType)))
        {
            storageDict[type] = 0;
        }
    }


    public virtual void AddResourceToInventory(Colony.ResourceType type, int amt)
    {
        storageDict[type] += amt;
    }
    public virtual void RemoveResourceFromInventory(Colony.ResourceType type, int amt)
    {
        if (storageDict.ContainsKey(type))
        {
            if (amt > storageDict[type])
            {
                return;
            }
            storageDict[type] -= amt;
            if (storageDict[type] < 0) storageDict[type] = 0;
        }
    }

    public bool HasResource(Colony.ResourceType type, int amt = 1)
    {
        if (storageDict.ContainsKey(type) && storageDict[type] >= amt)
        {
            return true;
        }
        return false;
    }

    public virtual void CreateFetchTask(Colony.ResourceType type)
    {
        // Create request for resource, FetchTask will created once request fulfilled
        Request newRequest = new Request(type, this, jobType);
        JobManager jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        outgoingRequests.Add(newRequest);
        jobManager.AddRequest(newRequest);
    }

    public virtual void CreateProduceTask()
    {
        ProduceTask newTask = new ProduceTask(this);
        JobManager jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        jobManager.AddTask(Colony.Job.Laborer,newTask);
    }

    public virtual void RecordTask(Task task)
    {
        outgoingTasks.Add(task);
    }


}   
