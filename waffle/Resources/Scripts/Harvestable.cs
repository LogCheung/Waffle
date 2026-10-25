using Godot;
using System;
using Godot.Collections;

public partial class Harvestable: StaticBody2D
{
    [Export] public Colony.Type type {get;set;} 

    [Export] public Vector2I tile {get;set;} 

    public static Dictionary<Vector2I,Harvestable> ObjectMap = new Dictionary<Vector2I, Harvestable>();
    private Sprite2D sprite;

    public override void _Ready()
    {
        ObjectMap.Add(this.tile, this);
        sprite = GetNode<Sprite2D>("Sprite2D");
        sprite.Frame = (int) type;
    }

    private void MatchType()
    {
        switch (type)
        {
            case Colony.Type.Wood:
                break;
            case Colony.Type.Stone:
                break;
        }
    }

    public void Remove()
    {
        ObjectMap.Remove(this.tile);
        QueueFree();
    }

}
