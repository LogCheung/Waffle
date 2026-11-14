using Godot;
using System;

public interface ProductionObject : TaskObject
{
    float progress {get;set;}


    void Produce()
    {
        GD.Print("how");
    }
}
