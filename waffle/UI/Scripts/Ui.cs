using Godot;
using System;

public partial class Ui : CanvasLayer
{
    private World world;

    public override void _Ready()
    {
        world = (World) GetTree().GetFirstNodeInGroup("WORLD");
    }


}
