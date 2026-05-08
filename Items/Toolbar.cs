using System.Linq;
using Godot;
using Godot.Collections;

public partial class Toolbar : Node
{
    public const int MaxItems = 9;

    [Signal]
    public delegate void ToolBarChangedEventHandler(Array<Item> newToolBar);

    [Signal]
    public delegate void ToolbarSelectedItemChangedEventHandler(int selectedIndex);

    [Export]
    public Array<Item> Items { get; private set; }

    public int selectedItemIndex = 0;

    // public Toolbar()
    // {
    //     Items = [.. Enumerable.Repeat(default(Item), MaxItems)];
    //     EmitSignal(SignalName.ToolBarChanged, Items);
    // }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ScrollUp"))
        {
            selectedItemIndex = (selectedItemIndex - 1 + MaxItems) % MaxItems;
            EmitSignal(SignalName.ToolbarSelectedItemChanged, selectedItemIndex);
        }
        else if (Input.IsActionJustPressed("ScrollDown"))
        {
            selectedItemIndex = (selectedItemIndex + 1) % MaxItems;
            EmitSignal(SignalName.ToolbarSelectedItemChanged, selectedItemIndex);
        }
    }
}