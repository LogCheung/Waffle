using Godot;
using System;

public partial class Harvestable: StaticBody2D
{
    [Export] public Colony.Type type;
    private Sprite2D sprite;

    public override void _Ready()
    {
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

}
