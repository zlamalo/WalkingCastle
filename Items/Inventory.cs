using System.Linq;
using Godot;
using Godot.Collections;

public partial class Inventory : Node
{
    public const int InventorySize = 36;
    public const int ToolbarSize = 9;

    [Signal]
    public delegate void InventoryChangedEventHandler(Array<Item> newInventory);

    [Signal]
    public delegate void ToolbarSelectedItemChangedEventHandler(int selectedIndex);

    public Array<Item> Items = [.. new Item[InventorySize]];
    public Array<Item> ToolbarItems => [.. Items.Take(ToolbarSize)];

    public int selectedItemIndex = 0;

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ScrollUp"))
        {
            selectedItemIndex = (selectedItemIndex - 1 + ToolbarSize) % ToolbarSize;
            EmitSignal(SignalName.ToolbarSelectedItemChanged, selectedItemIndex);
        }
        if (Input.IsActionJustPressed("ScrollDown"))
        {
            selectedItemIndex = (selectedItemIndex + 1) % ToolbarSize;
            EmitSignal(SignalName.ToolbarSelectedItemChanged, selectedItemIndex);
        }
        for (int i = 0; i < ToolbarSize; i++)
        {
            // Slightly scuffed, since it behaves like generated, but the actions must be defined
            // Right now there is action for Item0 to Item8
            if (Input.IsActionJustPressed($"Item{i}"))
            {
                selectedItemIndex = i;
                EmitSignal(SignalName.ToolbarSelectedItemChanged, selectedItemIndex);
            }
        }
    }

    public void AddItem(Item item, int? position = null)
    {
        // Duplicate the incoming item resource to avoid shared references
        var newItem = item.Duplicate() as Item ?? item;

        var pos = position ?? FindPositionForItem(newItem);
        if (pos >= 0)
        {
            if (newItem is StackableItem newStack && Items[pos] is StackableItem existingStack && existingStack.ItemResource.Id == newStack.ItemResource.Id)
            {
                existingStack.Quantity += newStack.Quantity;
            }
            else
            {
                Items[pos] = newItem;
            }

            EmitSignal(SignalName.InventoryChanged, Items.ToArray());
        }
    }

    public void RemoveItem(Item item)
    {
        if (item is StackableItem stackableItem)
        {
            var existingStacks = Items.OfType<StackableItem>().Where(x => x.ItemResource.Id == stackableItem.ItemResource.Id);
            foreach (var existingStack in existingStacks)
            {
                if (stackableItem.Quantity <= 0)
                    break;

                if (existingStack.Quantity > stackableItem.Quantity)
                {
                    existingStack.Quantity -= stackableItem.Quantity;
                    stackableItem.Quantity = 0;
                }
                else
                {
                    stackableItem.Quantity -= existingStack.Quantity;
                    Items[Items.IndexOf(existingStack)] = null;
                }
            }
        }
        else
        {
            var index = Items.IndexOf(item);
            if (index != -1)
            {
                Items[index] = null;
            }
        }

        EmitSignal(SignalName.InventoryChanged, Items.ToArray());
    }

    public int ContainsItemAmount(Item item)
    {
        if (item is StackableItem stackableItem)
        {
            var existingStacks = Items.OfType<StackableItem>().Where(x => x.ItemResource.Id == stackableItem.ItemResource.Id);
            if (existingStacks != null)
            {
                return existingStacks.Sum(x => x.Quantity);
            }
        }
        else
        {
            return Items.Count(x => x != null && x.ItemResource.Id == item.ItemResource.Id);
        }

        return 0;
    }

    private int FindPositionForItem(Item item)
    {
        if (item is StackableItem stackableItem)
        {
            var existingItem = Items.Where(x => x is StackableItem existingStack && existingStack.ItemResource.Id == stackableItem.ItemResource.Id)?.FirstOrDefault();
            if (existingItem != null)
            {
                return Items.IndexOf(existingItem);
            }
        }

        return Items.IndexOf(Items.FirstOrDefault(x => x == null));
    }
}