using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Helper : Node
{
    public static Node2D FindClosestNode(Vector2 pos, List<Node> nodeList)
    {
        if (nodeList.Count <= 0) return null;

        Node2D closest = null;

        foreach (Node2D node in nodeList)
        {
            if (node is not Node2D) continue;
            if (closest == null) closest = node;

            float curClosestDistance = pos.DistanceTo(closest.GlobalPosition);
            if (pos.DistanceTo(node.GlobalPosition) < curClosestDistance)
            {
                closest = node;
            }
        }
        return closest;
    }

    public List<Node> GetNodesInGroup(string groupName)
    {
        return GetTree().GetNodesInGroup(groupName).ToList();
    }

    public static Drop CreateDrop(Vector2 pos, Colony.ResourceType type, Node parent, bool visible = true, bool storageRetrieval = false)
    {
        PackedScene scene = GD.Load<PackedScene>("res://Resources/Scenes/drop.tscn");
        Drop drop = (Drop) scene.Instantiate();
            
        drop.GlobalPosition = pos;
        drop.type = type;
        drop.Visible = visible;
        drop.forStorageRetrieval = storageRetrieval;
        parent.AddChild(drop);

        return drop;
    }
}
