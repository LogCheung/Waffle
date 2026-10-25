using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Citizen : CharacterBody2D
{
    public enum Action
    {
        IDLE,
        TASK
    }
    public enum CombatAction
    {

    }
    [Export] public Colony.Job job { get; set; }
    [Export] public Task currentTask { get; set; }
    [Export] public Action currentAction { get; set; }
    [Export] public float speed {get;set;}
    public Vector2 targetPos {get;set;}
    private Timer wanderTimer;
    private JobManager jobManager;
    private NavigationAgent2D nav;

    public override void _Ready()
    {
        speed = 100.0f;

        wanderTimer = GetNode<Timer>("WanderTimer");
        jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        nav = GetNode<NavigationAgent2D>("NavigationAgent2D");

        wanderTimer.Timeout += OnWanderTimeout;
    }


    public override void _Process(double delta)
    {
        handleAction();
    }

    private void handleAction()
    {
        switch (currentAction)
        {
            case Action.IDLE:
                if (TryFindTask()) break;

                if (targetPos == default)
                {
                    targetPos = GetWanderDestination();
                }

                // reached destination case
                if (GlobalPosition.DistanceTo(targetPos) < 8)
                {
                    if (wanderTimer.TimeLeft <= 0) wanderTimer.Start();
                    return;
                }

                break;
            case Action.TASK:
                break;
        }

        handleMovement();
    }

    private void handleMovement()
    {
        if (targetPos == default) return;

        nav.TargetPosition = targetPos;

        Velocity = speed * GlobalPosition.DirectionTo(nav.GetNextPathPosition());

        MoveAndSlide();
    }

    private void OnWanderTimeout()
    {
        targetPos = GetWanderDestination();
    }
    private Vector2 GetWanderDestination()
    {
        Vector2 currentPos = GlobalPosition;
        float wanderDist = 128.0f;

        float randX = (float)GD.RandRange(-wanderDist, wanderDist);
        float randY = (float)GD.RandRange(-wanderDist, wanderDist);
        return currentPos + new Vector2(randX, randY);
    }

    private bool TryFindTask()
    {
        if (currentTask != null)
        {
            return true;
        }
        if (jobManager.TaskDict.GetValueOrDefault(job).Count >= 1)
        {
            currentTask = jobManager.TaskDict.GetValueOrDefault(job).First();
            currentAction = Action.TASK;
            return true;
        }

        return false;
    }


}
