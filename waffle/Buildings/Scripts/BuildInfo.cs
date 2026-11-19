using Godot;
using System;
using System.Collections.Generic;

public partial class BuildInfo : Node
{
    public static Dictionary<Colony.BuildType, Dictionary<Colony.ResourceType,int>> buildRecipes = new()
    {
        [Colony.BuildType.House] = new Dictionary<Colony.ResourceType,int>()
        {
            {Colony.ResourceType.Wood, 10},
            {Colony.ResourceType.Stone, 5}
        },
        [Colony.BuildType.FlourMill] = new Dictionary<Colony.ResourceType,int>()
        {
            {Colony.ResourceType.Wood, 5},
            {Colony.ResourceType.Stone, 5}
        },
        [Colony.BuildType.Bakery] = new Dictionary<Colony.ResourceType,int>()
        {
            {Colony.ResourceType.Wood, 15},
            {Colony.ResourceType.Stone, 0}
        },
        
        
    };

    public static Dictionary<Colony.BuildType, PackedScene> buildScenes = new()
    {
        [Colony.BuildType.House] = GD.Load<PackedScene>("res://Buildings/Scenes/housing.tscn"),
        [Colony.BuildType.FlourMill] = GD.Load<PackedScene>("res://Buildings/Scenes/workplace.tscn"),
        [Colony.BuildType.Bakery] = GD.Load<PackedScene>("res://Buildings/Scenes/workplace.tscn")
    };

    // Recipes
    public static Dictionary<Colony.BuildType, List<Recipe>> produceRecipes = new()
    {
        [Colony.BuildType.FlourMill] = new List<Recipe>()
        {
            // Selectable Recipes
            new Recipe(
            // inputs (type:amt)
            new Dictionary<Colony.ResourceType, int>()
            {
                {Colony.ResourceType.Wheat, 1}
            },
            // rewards (type:amt)
            new Dictionary<Colony.ResourceType, int>()
            {
                {Colony.ResourceType.Flour, 1}
            },
            // time
            1.0f)
        },
        [Colony.BuildType.Bakery] = new List<Recipe>()
        {
            // Selectable Recipes
            new Recipe(
            // inputs (type:amt)
            new Dictionary<Colony.ResourceType, int>()
            {
                {Colony.ResourceType.Flour, 1}
            },
            // rewards (type:amt)
            new Dictionary<Colony.ResourceType, int>()
            {
                {Colony.ResourceType.Bread, 1}
            },
            // time
            1.0f)
        },

    };

}
