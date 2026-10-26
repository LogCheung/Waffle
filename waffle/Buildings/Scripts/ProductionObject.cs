using Godot;
using System;

public partial class ProductionObject : StaticBody2D
{
    [Export] public Colony.Type type {get;set;} 
    [Export] public Colony.Job jobType {get;set;}

    [Export] public Vector2I tile {get;set;} 
    [Export] public float progress {get;set;}


    public virtual void Produce()
    {
        GD.Print("how");
    }
}
