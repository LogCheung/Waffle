using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

//[Tool]
public partial class TileWorld : TileMapLayer
{
    public enum ResourceType
    {
        Tree, 
        Stone, 
        Berries
    }

    public static Dictionary<ResourceType, Vector2I> ResourceTileMap = new Dictionary<ResourceType, Vector2I>()
    {
        {ResourceType.Tree, new Vector2I(2,2)},
        {ResourceType.Stone, new Vector2I(3,0)},
        {ResourceType.Berries, new Vector2I(3,1)}
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

        ResetWorld();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("ui_accept"))
        {
            ResetWorld();
        }
    }

    private void ResetWorld()
    {
        TileMapData = DefaultWorldData;
        GenerateClusters(4, 10, ForestCount, ResourceType.Tree);
        GenerateClusters(2, 4, 80, ResourceType.Stone);
        GenerateClusters(1, 3, 70, ResourceType.Berries);
    }

    private void GenerateForests()
    {
        // min size for a forest: 4x4
        List<Vector2I> usedCells = new List<Vector2I>();
        List<Vector2I> grassCells = GetUsedCellsById(0, new Vector2I(1,2)).ToList();
        for(int i = 0; i < ForestCount; i++)
        {
            int width = GD.RandRange(4, 10);
            int height = GD.RandRange(4, 10);
            int x = GD.RandRange(0, (int)WorldSize.X - width);
            int y = GD.RandRange(0, (int)WorldSize.Y - height);
    
            for(int j = 0; j < width; j++)
            {
                for(int k = 0; k < height; k++)
                {
                    if (GD.RandRange(0,10) == 0) continue; // add some randomness to the forest
                    usedCells.Add(new Vector2I(x + j,y + k));

                    // add randomness at edges
                    if (j == 0 || j == width - 1 || k == 0 || k == height - 1)
                    {
                        if (GD.RandRange(0,2) == 0) continue;
                        usedCells.Add(new Vector2I(x + j,y + k));
                    }
                }
            }
        }

        foreach (Vector2I cell in usedCells)
        {
            if (!grassCells.Contains(cell)) continue;
            SetCell(cell, 0, new Vector2I(2,2));
        }
    }

    private void GenerateClusters(int minDimension, int maxDimension,
    int clusterCount, ResourceType resourceType) 
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
}
