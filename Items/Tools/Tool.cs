using Godot;

[GlobalClass]
public partial class Tool : Item
{
    [Export]
    public ToolType Type { get; set; }
}
