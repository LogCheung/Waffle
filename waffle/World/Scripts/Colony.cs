using Godot;
using System;

public partial class Colony : Node
{
    public enum ResourceType
    {
        Wood, 
        Stone, 
        Berries,
        None
    }

    public enum BuildType {
        // correspond with InputButton types,
        // InputButton has additional types for non-building options
        None,
        House,
    }

    public enum Job
    {
        Laborer
    }
}
