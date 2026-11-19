using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Drop : StaticBody2D
{
    [Export] public float speed {get;set;}
    [Export] public float time {get;set;}
    [Export] public Colony.ResourceType type {get;set;}
    [Export] public bool taskCreatedForDrop {get;set;}
    [Export] public bool forStorageRetrieval {get;set;}
    private bool launching = false;
    private float timeSinceLaunch {get;set;}
    private Vector2 dir {get;set;}
    private Sprite2D sprite;
    private JobManager jobManager;
    public int ID {get;set;}
    public override async void _Ready()
    {
        ID = GD.RandRange(0,40000);
        jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        sprite = GetNode<Sprite2D>("Sprite2D");
        sprite.Frame = (int) type;
        
        if (forStorageRetrieval) return;
        Launch();
        if (TryFulfillRequest()) return;
        if (taskCreatedForDrop) return;


        // find Storage building
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        SendToNearestStorage();
    }

    public override void _Process(double delta)
    {
        if (launching)
        {
            GlobalPosition += dir * speed * (float) delta;
            timeSinceLaunch += (float) delta;
            if (timeSinceLaunch >= time) launching = false;
        }
    }

    private void Launch()
    {
        speed = 100.0f;
        time = .25f;

        dir = new Vector2((float) GD.RandRange(-1.0,1), (float) GD.RandRange(-1.0,1));
        timeSinceLaunch = 0;
        launching = true;
    }

    private void SendToNearestStorage()
    {
        if (TryFulfillRequest()) return;

        List<Node> nodes = GetTree().GetNodesInGroup("BUILDING").Where(x => x is StorageBuilding).ToList();
        Node node = Helper.FindClosestNode(GlobalPosition, nodes);
        StorageBuilding storage = node as StorageBuilding;
        storage.CreateTask(type, this);
    }

    private bool TryFulfillRequest()
    { 
        return jobManager.FulfillRequest(this);
    }
}
