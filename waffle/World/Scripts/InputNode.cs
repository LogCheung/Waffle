using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using System.Numerics;
using System.Reflection;

public partial class InputNode : Node
{
    [Signal] public delegate void InputTypeRequestedEventHandler(InputButton.Type type);
    [Export] public InputButton.Type type;
    private World world;
    private TileWorld tileWorld;
    private TileMapLayer hoverTiles;

    public override void _Ready()
    {
        world = (World) GetTree().GetFirstNodeInGroup("WORLD");
        tileWorld = (TileWorld) world.GetNode("TileWorld");
        hoverTiles = (TileMapLayer) tileWorld.GetChild(0);

        InputTypeRequested += OnInputTypeRequested;
    }

    private void OnInputTypeRequested(InputButton.Type type)
    {
        this.type = type;
        GD.Print(this.type);
    }

    public override void _Process(double delta)
    {
       HandleRequestedInput();
    }


    private Vector2I startTile = new Vector2I(-1,-1);
    private void HandleRequestedInput()
    {
        Action resultMethod;
        switch (this.type)
        {
            case InputButton.Type.SelectHarvest:
                resultMethod = () => HarvestTiles(hoverTiles.GetUsedCells());
                DragSelectedArea(resultMethod);
                break;
            default:
                break;
        }
    }

    private void DragSelectedArea(System.Action resultMethod)
    {
        Vector2I endTile = Vector2I.Zero;
        if (Input.IsActionJustPressed("select") && startTile == new Vector2I(-1,-1))
        {
            startTile = tileWorld.LocalToMap(tileWorld.GetLocalMousePosition());
        }
        if (Input.IsActionPressed("select"))
        {
            endTile = tileWorld.LocalToMap(tileWorld.GetLocalMousePosition());
            // highlight tiles
            HighlightTiles(startTile, endTile);
        }
        if (Input.IsActionJustReleased("select"))
        {
            startTile = new Vector2I(-1,-1);
            resultMethod?.Invoke();

            // putting tiles just to call function
            // but the arugments will not be used, it will just clear
            HighlightTiles(startTile,endTile,true);  
        }
    }

    private void HighlightTiles(Vector2I start, Vector2I end, bool clear = false)
    {
        // clear
        foreach (Vector2I cell in hoverTiles.GetUsedCells())
        {
            hoverTiles.SetCell(cell,-1);
        }
        if (clear) return;

        Array<Vector2I> cells = new Array<Vector2I>();

        int stepX = end.X < start.X ? -1 : 1;
        int stepY = end.Y < start.Y ? -1 : 1;
        int distX = Math.Abs(end.X - start.X);
        int distY = Math.Abs(end.Y - start.Y);

        for (int x = 0; x < distX ; x++)
        {
            for (int y = 0; y < distY ; y++)
            {
                cells.Add(start + new Vector2I(x * stepX,y * stepY));
            }
        }

        hoverTiles.SetCellsTerrainConnect(cells,0,0);
    }

    private void HarvestTiles(Array<Vector2I> cells)
    {
        foreach(Vector2I cell in cells)
        {
            if (Harvestable.ObjectMap.ContainsKey(cell))
            {
                Harvestable obj = Harvestable.ObjectMap.GetValueOrDefault(cell);
                obj.CreateTask();
            }
        }
    }

}
