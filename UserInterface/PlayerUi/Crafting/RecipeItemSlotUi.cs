using Godot;
using System;
using System.Linq;

public partial class RecipeItemSlotUi : Control
{
	private TextureRect itemIcon;
	private Label itemCountLabel;

	public override void _Ready()
	{
		itemIcon = GetNode<TextureRect>("%ItemIcon");
		itemCountLabel = GetNode<Label>("%ItemCountLabel");
		itemCountLabel.Text = "";
		itemCountLabel.Visible = false;
	}

	public void UpdateItem(Item recipeItem, string itemCountText = null, Color? textColor = null)
	{
		if (recipeItem == null)
			return;

		itemIcon.Texture = recipeItem.ItemResource.Icon;
		if (itemCountText != null)
		{
			itemCountLabel.Text = itemCountText;
			itemCountLabel.Visible = true;
			if (textColor.HasValue)
			{
				itemCountLabel.AddThemeColorOverride("font_color", textColor.Value);
			}
		}

		else
		{
			itemCountLabel.Visible = false;
		}
	}
}
