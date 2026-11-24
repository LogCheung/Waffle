using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class World : Node2D
{
    [Signal] public delegate void OnBuildingClickedEventHandler(Building building);

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
        //Engine.TimeScale = 8;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("ui_accept"))
        {
            //Engine.TimeScale = 2;
            //StartGame();
        }
    }
    private async void StartGame()
    {
        RemoveChildren(GetTree().GetFirstNodeInGroup("CITIZENSNODE")); // remove citizens
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
        tileWorld.SetCell(startTile,0,new Vector2I(0,1));

        Vector2 startPos = tileWorld.MapToLocal(startTile);
        cam.Position = startPos;
        AddStartingCitizens(startPos, 15);
        AddStartingBuilding(startPos + new Vector2(64,64));
        AddStartingBuilding(startPos - new Vector2(64,64));
    }

    private void AddStartingCitizens(Vector2 startPos, int count)
    {
        double distance = 100.0;
        int numOfCitizens = count;
        Node2D citizenNode = (Node2D) GetTree().GetFirstNodeInGroup("CITIZENSNODE");

        for (int i = 0; i < numOfCitizens; i++)
        {
            PackedScene scene = GD.Load<PackedScene>("res://Units/Scenes/citizen.tscn");
            Citizen citizen = (Citizen) scene.Instantiate();

            float randX = (float) GD.RandRange(-distance,distance);
            float randY = (float) GD.RandRange(-distance,distance);

            citizen.GlobalPosition = startPos + new Vector2(randX, randY);
            citizenNode.AddChild(citizen);
            
        } 
        
    }
    private void AddStartingBuilding(Vector2 startPos)
    {
        PackedScene scene = GD.Load<PackedScene>("res://Buildings/Scenes/storage_building.tscn");
        StorageBuilding building = (StorageBuilding) scene.Instantiate();
        Node2D buildingNode = (Node2D) GetTree().GetFirstNodeInGroup("BUILDINGNODE");

        building.GlobalPosition = startPos;
        building.tiles = new List<Vector2I>{tileWorld.LocalToMap(startPos)};
        buildingNode.AddChild(building);
    }
    private void RemoveChildren(Node node)
    {
        foreach (Node child in node.GetChildren())
        {
            child.QueueFree();
        }
    }


}
