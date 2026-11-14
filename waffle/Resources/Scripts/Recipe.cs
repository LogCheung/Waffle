using Godot;
using System;
using System.Collections.Generic;

public partial class Recipe : Node
{
    public Dictionary<Colony.ResourceType,int> rewards {get;set;}
    public Dictionary<Colony.ResourceType,int> inputs {get;set;}
    public float time;

    public Recipe(Dictionary<Colony.ResourceType,int> inputs, Dictionary<Colony.ResourceType,int> rewards, float time)
    {
        this.rewards = rewards;
        this.inputs = inputs;
        this.time = time;
    }
}
