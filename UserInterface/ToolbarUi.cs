using Godot;
using Godot.Collections;

public partial class ToolbarUi : Control
{
	private Array<ItemSlotUi> itemSlots = [];
	private int highlightedIndex = -1;

	public override void _Ready()
	{
		for (int i = 1; i <= Inventory.ToolbarSize; i++)
		{
			// suboptimal right now, there are physically placed nodes that match the count
			var itemSlot = GetNode<ItemSlotUi>($"ToolbarItems/ItemSlotUi{i}");
			itemSlots.Add(itemSlot);
		}

		GlobalNodes.Instance.PlayerReady += OnPlayerReady;
	}

	private void OnPlayerReady(Blacksmith player)
	{
		OnInventoryChanged(player.Inventory.Items);
		OnToolbarSelectedItemChanged(player.Inventory.selectedItemIndex);
		highlightedIndex = player.Inventory.selectedItemIndex;

		player.Inventory.InventoryChanged += OnInventoryChanged;
		player.Inventory.ToolbarSelectedItemChanged += OnToolbarSelectedItemChanged;
	}

	private void OnInventoryChanged(Array<Item> newItems)
	{
		for (int i = 0; i < itemSlots.Count; i++)
		{
			if (i < newItems.Count)
			{
				itemSlots[i].UpdateItem(newItems[i]);
			}
			else
			{
				itemSlots[i].UpdateItem(null);
			}
		}
	}

	private void OnToolbarSelectedItemChanged(int selectedIndex)
	{
		if (highlightedIndex != -1)
		{
			itemSlots[highlightedIndex].ToggleHighlight(false);
		}
		highlightedIndex = selectedIndex;
		itemSlots[selectedIndex].ToggleHighlight(true);
	}
}
