using Godot;
using System;

public partial class Colony : Node
{
    public enum ResourceType
    {
        Wood, 
        Stone, 
        Berries,
        Wheat,
        Flour,
        Bread,
        None
    }

    public enum BuildType {
        // correspond with InputButton types,
        // InputButton has additional types for non-building options
        None,
        House,
        FlourMill,
        Bakery,
    }

    public enum Job
    {
        Laborer,
        Farmer,

    }
}
