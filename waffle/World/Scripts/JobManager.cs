using Godot;
using System;
using System.Collections.Generic;

public partial class JobManager : Node
{
    public Dictionary<Colony.Job,List<Task>> TaskDict = new Dictionary<Colony.Job, List<Task>>();

    public override void _Ready()
    {
        foreach (Colony.Job job in Enum.GetValues(typeof(Colony.Job)))
        {
            TaskDict.Add(job, new List<Task>());
        }
    }


    public void AddTask(Colony.Job job, Task task)
    {
        TaskDict.GetValueOrDefault(job).Add(task);
    }
}
