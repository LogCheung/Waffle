using Godot;
using System;

public partial class Task : Node
{
    public enum Type
    {
        FETCH,
        PRODUCE
    }
     [Export] public StaticBody2D sourceObject {get;set;} 
     [Export] public Type type {get;set;}
     [Export] public Citizen worker {get;set;}
     [Export] public bool failed {get;set;} = false;

     public Vector2 targetPos {get;set;}

     public Task(StaticBody2D source)
    {
        sourceObject = source;
    }
}
