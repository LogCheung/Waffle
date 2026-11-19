using Godot;
using System;

public interface TaskObject
{
    Colony.Job jobType {get;set;}
    Vector2I tile {get;set;} 


    void Produce(){}
}
