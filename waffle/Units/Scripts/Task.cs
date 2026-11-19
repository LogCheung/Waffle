using Godot;
using System;
using System.Collections.Generic;

public partial class Task : Node
{
    public enum Type
    {
        FETCH,
        PRODUCE
    }
    public enum Priority
    {
        HIGH,
        LOW,
    }
    public static List<int> TaskIDs = new List<int>();
    public int ID {get;set;}
     [Export] public StaticBody2D sourceObject {get;set;} 
     [Export] public Priority priority {get;set;} = Priority.HIGH;
     [Export] public Type type {get;set;}
     [Export] public Citizen worker {get;set;}
     [Export] public bool failed {get;set;} = false;

     public Vector2 targetPos {get;set;}

     public Task(StaticBody2D source)
    {
        ID = GD.RandRange(0,40000);
        while (TaskIDs.Contains(ID))
        {
            ID = GD.RandRange(0,4000);
        }
        TaskIDs.Add(ID);

        sourceObject = source;
    }
}
