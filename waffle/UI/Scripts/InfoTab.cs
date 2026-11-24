using Godot;
using System;

public partial class InfoTab : Control
{
    [Export] public Building shownBuilding;

    private Label label;
    private HBoxContainer hContainer;
    public override void _Ready()
    {
        label = GetNode<Label>("Label");
        hContainer = GetNode<HBoxContainer>("HBoxContainer");

        Visible = false;
    }

    public void Update(Building building)
    {
        if (building == shownBuilding) // if current shown building is same
        {
            Visible = false;
            shownBuilding = null;
            return;
        }
        if (!(building is Workplace)) return;

        // reset container
        foreach (Node c in hContainer.GetChildren())
        {
            c.QueueFree();
        }

        Visible = true;
        shownBuilding = building;
        Workplace shownWorkplace = building as Workplace;
        label.Text = building.buildType.ToString();
        for (int i = 0; i < shownWorkplace.maxWorkers; i++)
        {
            PackedScene scene = GD.Load<PackedScene>("res://UI/Scenes/WorkerButton.tscn");
            WorkerButton button = (WorkerButton) scene.Instantiate();
            button.workplace = shownWorkplace;
            button.jobType = shownWorkplace.jobType;
            hContainer.AddChild(button);
        }

    }

}
