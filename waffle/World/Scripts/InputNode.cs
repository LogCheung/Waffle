using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class InputNode : Node
{
    [Signal] public delegate void InputTypeRequestedEventHandler(InputButton.Type type);

    [Export] public InputButton.Type type;
    [Export] public bool dragging = false;
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
    }

    public override void _Process(double delta)
    {
       HandleRequestedInput();
    }


    private Vector2I startTile = new Vector2I(-1,-1);
    private void HandleRequestedInput()
    {
        Action resultMethod;
        Vector2I startTile = new Vector2I(-1,-1);
        int xDim = 0;
        int yDim = 0;
        switch (this.type)
        {
            case InputButton.Type.SelectHarvest:
                resultMethod = () => HarvestTiles(hoverTiles.GetUsedCells());
                DragSelectedArea(resultMethod);
                dragging = true;
                break;
            case InputButton.Type.House:
            case InputButton.Type.FlourMill:
            case InputButton.Type.Bakery:
                dragging = false;
                xDim = BuildSettings.settings[this.type]["xDim"];
                yDim = BuildSettings.settings[this.type]["yDim"];
                startTile = tileWorld.LocalToMap(tileWorld.GetLocalMousePosition() + new Godot.Vector2(32,32));
                startTile += new Vector2I(-xDim/2,-yDim/2); // offset
                HighlightArea(xDim,yDim,startTile);
                break;
            default:
                // basic selection
                Vector2I tile = tileWorld.LocalToMap(tileWorld.GetLocalMousePosition());
                if (Building.ObjectMap.ContainsKey(tile)) 
                resultMethod = () => GD.Print("No input type selected");
                return;
        }

        if (Input.IsActionJustPressed("select") && !dragging)
        {
            startTile -= new Vector2I(-xDim/2,-yDim/2);
            resultMethod = () => CreateBuilding(xDim,yDim, startTile);
            resultMethod?.Invoke();
            OnInputCompleted();
        }

        if (Input.IsActionJustPressed("cancel"))
        {
           OnInputCompleted();
        }


    }

    private void OnInputCompleted()
    {
        ClearHoverTiles();
        dragging = false;
        type = InputButton.Type.None;
    }

    private void HighlightArea(int xDim, int yDim, Vector2I tile)
    {
        // clear tiles
        ClearHoverTiles();

        Array<Vector2I> cells = new Array<Vector2I>();

        for (int x = 0; x < xDim ; x++)
        {
            for (int y = 0; y < yDim ; y++)
            {
                cells.Add(tile + new Vector2I(x,y));
            }
        }

        hoverTiles.SetCellsTerrainConnect(cells,0,0);
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
            DragHighlightTiles(startTile, endTile);
        }
        if (Input.IsActionJustReleased("select"))
        {
            startTile = new Vector2I(-1,-1);
            resultMethod?.Invoke();

            // putting tiles just to call function
            // but the arugments will not be used, it will just clear
            DragHighlightTiles(startTile,endTile,true);  
        }
    }

    private void DragHighlightTiles(Vector2I start, Vector2I end, bool clear = false)
    {
        // clear
        ClearHoverTiles();
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

    private void ClearHoverTiles()
    {
        foreach (Vector2I cell in hoverTiles.GetUsedCells())
        {
            hoverTiles.SetCell(cell,-1);
        }
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

    private void CreateBuilding(int xDim, int yDim, Vector2I tile)
    {
        PackedScene scene = GD.Load<PackedScene>("res://Buildings/Scenes/construction.tscn");
        Construction construction = (Construction) scene.Instantiate();
        Node2D buildingNode = (Node2D) world.GetNode("BuildingNode");
        construction.inputBuildType = this.type;
        construction.GlobalPosition = tileWorld.MapToLocal(tile) - new Godot.Vector2(32,32);

        buildingNode.AddChild(construction);
    }

    private void ShowBuildingInfo(Vector2I tile)
    {
        Building building = Building.ObjectMap[tile];
        world.EmitSignal(World.SignalName.OnBuildingClicked,building);
    }



}
