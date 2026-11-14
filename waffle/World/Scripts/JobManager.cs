using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class JobManager : Node
{
    public Dictionary<Colony.Job,List<Task>> TaskDict = new Dictionary<Colony.Job, List<Task>>();
    public List<Request> RequestList {get;set;} = new List<Request>();
    public Dictionary<Colony.Job,List<Citizen>> WorkerDict = new Dictionary<Colony.Job, List<Citizen>>();
    

    public override void _Ready()
    {
        foreach (Colony.Job job in Enum.GetValues(typeof(Colony.Job)))
        {
            TaskDict.Add(job, new List<Task>());
        }
    }

    public override void _Process(double delta)
    {
        //GD.Print(string.Join(", ", TaskDict.Select(kvp => $"{kvp.Key}={kvp.Value.Count}")));
    }


    public void AddTask(Colony.Job job, Task task)
    {   
        TaskDict.GetValueOrDefault(job).Add(task);
    }

    public void AddRequest(Request request)
    {
        
        RequestList.Add(request);
    }
}
