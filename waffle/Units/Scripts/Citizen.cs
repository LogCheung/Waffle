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
    [Export] public Drop drop {get;set;}
    [Export] public Housing home {get;set;}
    private Timer wanderTimer;
    private JobManager jobManager;
    private NavigationAgent2D nav;
    private PopulationManager populationManager;

    public override void _Ready()
    {
        speed = 100.0f;

        wanderTimer = GetNode<Timer>("WanderTimer");
        jobManager = (JobManager) GetTree().GetFirstNodeInGroup("JOBMANAGER");
        populationManager = (PopulationManager) GetTree().GetFirstNodeInGroup("POPULATIONMANAGER");
        nav = GetNode<NavigationAgent2D>("NavigationAgent2D");

        wanderTimer.Timeout += OnWanderTimeout;

        populationManager.citizens.Add(this);
    }


    public override void _Process(double delta)
    {
        handleAction();
        if (currentTask != null) GD.Print(currentTask.ID);
    }

    private void handleAction()
    {
        switch (currentAction)
        {
            case Action.IDLE:
                if (TryFindTask()) break;
                if (nav.TargetPosition == default) nav.TargetPosition = GetWanderDestination();

                // reached destination case
                if (GlobalPosition.DistanceTo(nav.TargetPosition) < 8)
                {
                    if (wanderTimer.TimeLeft <= 0) wanderTimer.Start();
                    return;
                }

                break;
            case Action.TASK:
                if (currentTask == null)
                {
                    currentAction = Action.IDLE;
                    return;
                }
                // carry resource overhead
                if (!IsInstanceValid(drop)) drop = null;
                if (drop != null) drop.GlobalPosition = GlobalPosition + new Vector2(0,-30);


                // reached destination case
                
                if (nav.IsNavigationFinished())
                {
                    if (currentTask is ProduceTask)
                    {
                        ProduceTask task = currentTask as ProduceTask;
                        task.AddProgress(0.5f);
                    }
                    if (currentTask is FetchTask)
                    {
                        FetchTask task = currentTask as FetchTask;
                        if (!task.resourceRetrieved)  {
                            task.OnReosurceRetrieved();
                            nav.TargetPosition = currentTask.targetPos;
                        }
                        else task.DeliverResource();
                    }
                }

                break;
        }

        handleMovement();
    }

    private Vector2 nextPathPos = Vector2.Zero;
    private void handleMovement()
    {
        if (nav.TargetPosition == default) return;


        Velocity = speed * GlobalPosition.DirectionTo(GetNextPathPos());

        MoveAndSlide();
    }
    private Vector2 GetNextPathPos()
    {
        if (nextPathPos == default) nextPathPos = nav.GetNextPathPosition();
        if (GlobalPosition.DistanceTo(nextPathPos) < 8)
        {
            nextPathPos = nav.GetNextPathPosition();
        } 

        return nextPathPos;
    }


    private void OnWanderTimeout()
    {
        nav.TargetPosition = GetWanderDestination();
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
            if (currentTask.failed || currentTask.worker != null) { // remove and become idle if chosen task is invalid
                currentTask = null;
                currentAction = Action.IDLE;
                return false;
            }

            jobManager.TaskDict.GetValueOrDefault(job).Remove(currentTask);
            wanderTimer.Stop();
            currentTask.worker = this;
            currentAction = Action.TASK;
            nav.TargetPosition = currentTask.targetPos;
            return true;
        }

        currentAction = Action.IDLE;
        return false;
    }


}
