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
}
