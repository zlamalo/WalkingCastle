using Godot;
using Godot.Collections;

public partial class ToolbarUi : Control
{
	private Array<ItemSlotUi> itemSlots = [];
	private int highlightedIndex = -1;

	public override void _Ready()
	{
		for (int i = 1; i <= 9; i++)
		{
			// suboptimal right now, there are physically placed nodes that match the count
			var itemSlot = GetNode<ItemSlotUi>($"ToolbarItems/ItemSlotUi{i}");
			itemSlots.Add(itemSlot);
		}

		GlobalNodes.Instance.PlayerReady += OnPlayerReady;
	}

	private void OnPlayerReady(Blacksmith player)
	{
		OnToolBarChanged(player.Toolbar.Items);
		OnToolbarSelectedItemChanged(player.Toolbar.selectedItemIndex);
		highlightedIndex = player.Toolbar.selectedItemIndex;

		player.Toolbar.ToolBarChanged += OnToolBarChanged;
		player.Toolbar.ToolbarSelectedItemChanged += OnToolbarSelectedItemChanged;
	}

	private void OnToolBarChanged(Array<Item> newToolBarItems)
	{
		for (int i = 0; i < itemSlots.Count; i++)
		{
			if (i < newToolBarItems.Count)
			{
				itemSlots[i].ItemSprite.Texture = newToolBarItems[i]?.Icon;
			}
			else
			{
				itemSlots[i].ItemSprite.Texture = null;
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
