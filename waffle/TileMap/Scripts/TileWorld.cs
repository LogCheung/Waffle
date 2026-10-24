using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

//[Tool]
public partial class TileWorld : TileMapLayer
{

    public static Dictionary<Colony.Type, Vector2I> ResourceTileMap = new Dictionary<Colony.Type, Vector2I>()
    {
        {Colony.Type.Wood, new Vector2I(2,2)},
        {Colony.Type.Stone, new Vector2I(3,0)},
        {Colony.Type.Berries, new Vector2I(3,1)}
    };


    [Export] public int ForestCount {get;set;} = 160;
    public Vector2 WorldSize {get;set;}
    public AStarGrid2D AStar {get;set;}
    public byte[] DefaultWorldData {get;set;}
    public override void _Ready()
    {
        DefaultWorldData = TileMapData;
        AStar = new AStarGrid2D();
        AStar.Region = GetUsedRect();
        AStar.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
        AStar.DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan;
        AStar.Update();
        WorldSize = new Vector2(128,128);

        // map generation called in world.cs
    }

    public void ResetWorld()
    {
        TileMapData = DefaultWorldData;

        Node2D harvestablesNode = (Node2D) GetTree().GetFirstNodeInGroup("HARVESTABLESNODE");
        foreach (Node child in harvestablesNode.GetChildren()) {
            child.QueueFree();
        }



        GenerateClusters(4, 10, ForestCount, Colony.Type.Wood);
        GenerateClusters(3, 5, 80, Colony.Type.Stone);
        GenerateClusters(1, 3, 70, Colony.Type.Berries);

        GenerateSprinkles(200, Colony.Type.Wood);
        GenerateSprinkles(200, Colony.Type.Stone);
        GenerateSprinkles(60, Colony.Type.Berries);

        GenerateHarvestables();
    }


    private void GenerateClusters(int minDimension, int maxDimension,
    int clusterCount, Colony.Type resourceType) 
    {
        Vector2I atlas = ResourceTileMap[resourceType];

        // min size for a forest: 4x4
        List<Vector2I> usedCells = new List<Vector2I>();
        List<Vector2I> waterCells = GetUsedCellsById(0, new Vector2I(0,2)).ToList();
        for(int i = 0; i < clusterCount; i++)
        {
            int width = GD.RandRange(minDimension, maxDimension);
            int height = GD.RandRange(minDimension, maxDimension);
            int x = GD.RandRange(0, (int)WorldSize.X - width);
            int y = GD.RandRange(0, (int)WorldSize.Y - height);
    
            for(int j = 0; j < width; j++)
            {
                for(int k = 0; k < height; k++)
                {
                    if (GD.RandRange(0,10) == 0) continue; // add some randomness to the forest
                    usedCells.Add(new Vector2I(x + j,y + k));
                }
            }
        }

        foreach (Vector2I cell in usedCells)
        {
            if (waterCells.Contains(cell)) continue;
            SetCell(cell, 0, atlas);
        }
    }
    private void GenerateSprinkles(int count, Colony.Type resourceType)
    {
        Vector2I atlas = ResourceTileMap[resourceType];
        List<Vector2I> grassCells = GetGrassCells();

        for (int i = 0; i < count; i++)
        {
            if (grassCells.Count <= 0) break;
            int index = GD.RandRange(0, grassCells.Count);

            try 
            {
                SetCell(grassCells[index], 0, atlas);
            }
            catch (Exception e)
            {
                GD.PrintErr($"Error setting cell : {e.Message}");
            }


            grassCells.RemoveAt(index);
        }
    }

    private void GenerateHarvestables()
    {
        List<Vector2I> usedCells;
        foreach (Colony.Type key in ResourceTileMap.Keys) {
            usedCells = GetUsedCellsById(0,ResourceTileMap.GetValueOrDefault(key)).ToList();

            foreach (Vector2I cell in usedCells)
            {
                String path = "res://Resources/Scenes/harvestable.tscn";
                PackedScene scene = GD.Load<PackedScene>(path);
                Harvestable newNode = scene.Instantiate<Harvestable>();
                Node2D parentNode = (Node2D) GetTree().GetFirstNodeInGroup("HARVESTABLESNODE");

                newNode.GlobalPosition = MapToLocal(cell);
                newNode.type = key;
                parentNode.AddChild(newNode);
            }
        }

        
    }

    public List<Vector2I> GetGrassCells()
    {
        return GetUsedCellsById(0, new Vector2I(1,2)).ToList();
    }
    
}
