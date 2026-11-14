using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Workplace : Building, ProductionObject
{
    [Export] public Recipe recipe {get;set;}
    [Export] public float progress {get;set;}
    [Export] public int maxProductCount {get;set;}
    [Export] public int maxInputCount {get;set;}
    [Export] public int inputThreshold {get;set;}
    public override void _Ready()
    {
        base._Ready();
    }

    public void Produce()
    {
        // create drops (invisible drops)
        World world = (World) GetTree().GetFirstNodeInGroup("WORLD");
        List<Colony.ResourceType> rewardTypes = recipe.rewards.Keys.ToList();
        foreach (Colony.ResourceType r in rewardTypes)
        {
            for (int i = 0; i < recipe.rewards[r]; i++)
            {
                Helper.CreateDrop(GlobalPosition,r,world,false);
            }
        }

        // recurring task

    }

    private void CreateDemand()
    {
        List<Colony.ResourceType> inputTypes = recipe.inputs.Keys.ToList();
        foreach (Colony.ResourceType r in inputTypes)
        {
            for (int i = 0; i < inputThreshold;i++)
            {
                CreateFetchTask(r);
            }
        }
    }


}
