using Godot;
using Godot.Collections;

public partial class ToolbarUi : PanelContainer
{
	private PackedScene itemSlotScene = GD.Load<PackedScene>("res://UserInterface/ItemSlotUi.tscn");

	private HBoxContainer toolbarItems;

	private Array<ItemSlotUi> itemSlots = [];
	private int highlightedIndex = -1;

	public override void _Ready()
	{
		toolbarItems = GetNode<HBoxContainer>("%ToolbarItems");
		for (int i = 1; i <= Inventory.ToolbarSize; i++)
		{
			var itemSlot = itemSlotScene.Instantiate<ItemSlotUi>();
			toolbarItems.AddChild(itemSlot);
			itemSlots.Add(itemSlot);
		}

		GlobalNodes.Instance.PlayerReady += OnPlayerReady;
	}

	private void OnPlayerReady(Player player)
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
