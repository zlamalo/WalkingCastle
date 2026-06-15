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

    public void AddItem(Item item, int? position = null)
    {
        item = item.Duplicate() as Item; // Duplicate the item to avoid reference issues
        var pos = position ?? FindPositionForitem(item);
        if (pos >= 0)
        {
            if (Items[pos] != null && Items[pos].Name == item.Name && item is StackableItem stackableItem)
            {
                var existingStack = Items[pos] as StackableItem;
                existingStack.Quantity += stackableItem.Quantity;
            }
            else
            {
                Items[pos] = item;
            }

            EmitSignal(SignalName.ToolBarChanged, Items);
        }
    }

    private int FindPositionForitem(Item item)
    {
        if (item is StackableItem stackableItem)
        {
            var existingItem = Items.Where(x => x != null && x.Name == stackableItem.Name)?.FirstOrDefault();
            if (existingItem != null)
            {
                return Items.IndexOf(existingItem);
            }
        }
        // empty slot index
        return Items.IndexOf(Items.Where(x => x == null).FirstOrDefault());
    }
}