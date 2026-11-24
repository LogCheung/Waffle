using Godot;
using System;

public partial class Ui : CanvasLayer
{
    private World world;
    private InfoTab infoTab;

    public override void _Ready()
    {
        world = (World) GetTree().GetFirstNodeInGroup("WORLD");
        infoTab = GetNode<InfoTab>("InfoTab");

        world.OnBuildingClicked += _OnBuildingClicked;
    }


    private void _OnBuildingClicked(Building building)
    {
        infoTab.Update(building);
    }


}
