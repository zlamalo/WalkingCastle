using Godot;

[GlobalClass]
public partial class Tool : ItemResource
{
    [Export]
    public ToolType Type { get; set; }
}
