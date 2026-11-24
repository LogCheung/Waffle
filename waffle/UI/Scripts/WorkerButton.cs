using Godot;
using System;
using System.Linq;

public partial class WorkerButton : Button
{
    public Workplace workplace {get;set;}
    public Colony.Job jobType {get;set;}
    public Citizen citizen {get;set;}

    private Sprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
        base._Ready();

        this.Pressed += () => OnButtonPressed();
    }

    private void OnButtonPressed()
    {

        if (ButtonPressed)
        {
            if (JobManager.WorkerDict[Colony.Job.Laborer].Count <= 0 ) return;
            // find laborer to assign
            citizen = JobManager.WorkerDict[Colony.Job.Laborer].First();
            citizen.AssignJob(this.jobType,workplace);
            // perhaps in future make it so its the closest unassigned citizen
        }
        else
        {
            citizen.AssignJob(Colony.Job.Laborer,workplace);
            citizen = null;
        }

        sprite.Frame = ButtonPressed ? 1 : 0;
        Modulate = ButtonPressed ? new Color(1.5f,1.5f,1.5f) : new Color(1f,1f,1f);
    }

}
