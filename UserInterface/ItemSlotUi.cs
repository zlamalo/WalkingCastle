using Godot;
using System;

public partial class ItemSlotUi : Panel
{
	private NinePatchRect border;
	private Sprite2D itemSprite;
	private Label itemCountLabel;

	public bool Highlighted { get; set; } = false;

	public override void _Ready()
	{
		border = GetNode<NinePatchRect>("Border");
		itemSprite = GetNode<Sprite2D>("Sprite2D");
		itemCountLabel = GetNode<Label>("ItemCountLabel");
	}

	public void UpdateItem(Item item)
	{
		if (item != null)
		{
			itemSprite.Texture = item.Icon;
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
			border.SelfModulate = Colors.LimeGreen;
		}
		else
		{
			border.SelfModulate = Colors.White;
		}
	}
}
