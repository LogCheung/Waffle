using Godot;
using System;

public partial class Task : Node
{
    public enum Type
    {
        FETCH,
        PRODUCE
    }
     [Export] public ProductionObject sourceObject {get;set;} 
     [Export] public Type type {get;set;}
     [Export] public Citizen worker {get;set;}
     [Export] public bool failed {get;set;}

     public Vector2 targetPos {get;set;}

     public Task(ProductionObject source)
    {
        sourceObject = source;
        this.type = type;
    }
}
