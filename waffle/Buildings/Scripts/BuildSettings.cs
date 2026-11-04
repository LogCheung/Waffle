using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class BuildSettings : Resource
{
    public static Dictionary<InputButton.Type, Dictionary<String,int>> settings = new Dictionary<InputButton.Type, Dictionary<String,int>>()
    {
        {InputButton.Type.House, new Dictionary<String,int>() { {"xDim", 2}, {"yDim", 2} }},
    };
    
    [Export] public InputButton.Type type;
}
