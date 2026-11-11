
using Godot;
using System;
using System.Collections.Generic;

public partial class Construction : Building, ProductionObject
{
    [Export] public InputButton.Type inputBuildType {get; set;}
    [Export] public Colony.ResourceType type {get;set;} = Colony.ResourceType.None;
    [Export] public float progress {get;set;}


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
                CreateFetchTask(kvp.Key);
            }
        }
    }

    public override void AddResourceToInventory(Colony.ResourceType type, int amt)
    {
        base.AddResourceToInventory(type, amt);
        if (CheckIfConstructionComplete())
        {
            CreateProduceTask();
        }
    }

    private bool CheckIfConstructionComplete()
    {
        foreach (KeyValuePair<Colony.ResourceType,int> kvp in requiredResources)
        {
            if (!storageDict.ContainsKey(kvp.Key) || storageDict[kvp.Key] < kvp.Value)
            {
                return false;
            }
        }
        return true;
    }

    public void Produce()
    {
        PackedScene scene = BuildInfo.buildScenes[buildType];
        Building newBuilding = (Building) scene.Instantiate();
        newBuilding.GlobalPosition = GlobalPosition;
        GetTree().GetFirstNodeInGroup("BUILDINGNODE").AddChild(newBuilding);
        QueueFree();
    }



}
