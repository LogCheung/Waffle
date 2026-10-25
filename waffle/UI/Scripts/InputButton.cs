using Godot;
using System;

public partial class InputButton : Button
{
    public enum Type {
        None,
        SelectHarvest,

    }
    [Export] public Type type;

    private InputNode inputNode;

    public override void _Ready()
    {
        inputNode = (InputNode) GetTree().GetFirstNodeInGroup("INPUTNODE");
        
        Connect(SignalName.Pressed, Callable.From(OnButtonPressed));
    }

    private void OnButtonPressed()
    {
        inputNode.EmitSignal(InputNode.SignalName.InputTypeRequested, (int) this.type);
    }


}
