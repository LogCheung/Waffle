using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Building : ProductionObject
{
    protected Dictionary<Colony.ResourceType,int> storageDict = new Dictionary<Colony.ResourceType, int>();

    public override void _Ready()
    {
        foreach (Colony.ResourceType type in Enum.GetValues(typeof(Colony.ResourceType)))
        {
            storageDict[type] = 0;
        }
    }

    public virtual void AddResourceToInventory(Colony.ResourceType type, int amt)
    {
        GD.Print($"adding {amt} {type} to inventory");
        GD.Print(storageDict.ContainsKey(type));
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

    public virtual void CreateTask(Colony.ResourceType type)
    {
        FetchTask newTask = new FetchTask(this, type);
        JobManager jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        jobManager.AddTask(Colony.Job.Laborer,newTask);
    }


}   
