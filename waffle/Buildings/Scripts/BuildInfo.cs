using Godot;
using System;
using System.Collections.Generic;

public partial class BuildInfo : Node
{
    public static Dictionary<Colony.BuildType, Dictionary<Colony.ResourceType,int>> recipes = new()
    {
        [Colony.BuildType.House] = new Dictionary<Colony.ResourceType,int>()
        {
            {Colony.ResourceType.Wood, 10},
            {Colony.ResourceType.Stone, 5}
        },
        
    };

    public static Dictionary<Colony.BuildType, PackedScene> buildScenes = new()
    {
        [Colony.BuildType.House] = GD.Load<PackedScene>("res://Buildings/Scenes/housing.tscn")
    };

}
