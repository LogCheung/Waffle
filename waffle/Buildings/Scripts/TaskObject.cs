using Godot;
using System;
using System.Collections.Generic;

public interface TaskObject
{
    Colony.Job jobType {get;set;}
    List<Vector2I> tiles {get;set;} 


    void Produce(){}
}
