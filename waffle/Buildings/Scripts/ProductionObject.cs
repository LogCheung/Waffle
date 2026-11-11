using Godot;
using System;

public interface ProductionObject : TaskObject
{
    Colony.ResourceType type {get;set;} 
    float progress {get;set;}


    void Produce()
    {
        GD.Print("how");
    }
}
