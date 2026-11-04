
using Godot;
using System;
using System.Collections.Generic;

public partial class Construction : Building
{
    [Export] public InputButton.Type inputBuildType {get; set;}


    private Colony.BuildType buildType;
    private Dictionary<Colony.ResourceType,int> requiredResources;

    public override void _Ready()
    {
        base._Ready();
        buildType = (Colony.BuildType) inputBuildType;
        requiredResources = BuildInfo.recipes[buildType];

        StartConstruction();
    }

    private void StartConstruction()
    {
        foreach (KeyValuePair<Colony.ResourceType,int> kvp in requiredResources)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                CreateTask(kvp.Key);
            }
        }
    }

    public override void AddResourceToInventory(Colony.ResourceType type, int amt)
    {
        base.AddResourceToInventory(type, amt);
        if (CheckIfConstructionComplete())
        {
            GD.Print("construction complete");
            //buildInfo.EmitSignal("ConstructionComplete", this);
        }
    }

    private bool CheckIfConstructionComplete()
    {
        foreach (KeyValuePair<Colony.ResourceType,int> kvp in requiredResources)
        {
            if (!storageDict.ContainsKey(kvp.Key) || storageDict[kvp.Key] < kvp.Value)
            {
                GD.Print($"not enough {kvp.Key}: {storageDict.GetValueOrDefault(kvp.Key,0)}/{kvp.Value}");
                return false;
            }
        }

        GD.Print("omg");
        return true;
    }

}
