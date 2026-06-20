using Godot;
using System;

public partial class ItemSlotUi : PanelContainer
{
	//private NinePatchRect border;
	private TextureRect itemSprite;
	private Label itemCountLabel;

	public bool Highlighted { get; set; } = false;

	public override void _Ready()
	{
		//border = GetNode<NinePatchRect>("Border");
		itemSprite = GetNode<TextureRect>("%ItemSprite");
		itemCountLabel = GetNode<Label>("ItemCountLabel");
	}

	public void UpdateItem(Item item)
	{
		if (item != null && item.ItemResource != null)
		{
			itemSprite.Texture = item.ItemResource.Icon;
			if (item is StackableItem stackableItem)
			{
				itemCountLabel.Text = stackableItem.Quantity.ToString();
				itemCountLabel.Visible = true;
			}
			else
			{
				itemCountLabel.Visible = false;
			}
		}
		else
		{
			itemSprite.Texture = null;
			itemCountLabel.Visible = false;
		}
	}

	public void ToggleHighlight(bool highlight)
	{
		Highlighted = highlight;
		if (Highlighted)
		{
			SelfModulate = Colors.LimeGreen;
		}
		else
		{
			SelfModulate = Colors.White;
		}
	}
}
