using Godot;
using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

public partial class World : Node2D
{
    private TileWorld tileWorld;
    private Camera2D cam;
    public Vector2I startTile = new Vector2I(0,0);
    private bool validStartTile = false;
    public override void _Ready()
    {
        tileWorld = GetNode<TileWorld>("TileWorld");
        cam = GetNode<Camera2D>("Camera2D");

        StartGame();
    }

    public override void _Process(double delta)
    {
        
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("ui_accept"))
        {
            StartGame();
        }
    }
    private async void StartGame()
    {
        tileWorld.ResetWorld();

        List<Vector2I> grassCells = tileWorld.GetGrassCells();

        // find valid starting tile
        validStartTile = false;
        startTile = new Vector2I(GD.RandRange(20,100), GD.RandRange(20,100));
        while(!validStartTile)
        {
            // check if the start tile is on grass
            if (!grassCells.Contains(startTile))
            {
                startTile = new Vector2I(GD.RandRange(20,100), GD.RandRange(20,100));
                continue;
            }

            /* check neighbors to ensure a good space for start 
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    Vector2I neighborTile = startTile + new Vector2I(i, j);
                    if (!grassCells.Contains(neighborTile))
                    {
                        startTile = new Vector2I(GD.RandRange(20,100), GD.RandRange(20,100));
                        continue;
                    }
                }
            }
            */
            
            validStartTile = true;
        }
        GD.Print(tileWorld.GetCellAtlasCoords(startTile));
        tileWorld.SetCell(startTile,0,new Vector2I(0,1));

        cam.Position = tileWorld.MapToLocal(startTile);

    }

}
