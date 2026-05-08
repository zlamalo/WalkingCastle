using Godot;
using System;

public partial class ItemSlotUi : Panel
{
	private NinePatchRect border;

	public Sprite2D ItemSprite { get; private set; }
	public bool Highlighted { get; set; } = false;

	public override void _Ready()
	{
		border = GetNode<NinePatchRect>("Border");
		ItemSprite = border.GetNode<Sprite2D>("Sprite2D");
	}

	public void ToggleHighlight(bool highlight)
	{
		Highlighted = highlight;
		if (Highlighted)
		{
			border.SelfModulate = Colors.Red;
		}
		else
		{
			border.SelfModulate = Colors.White;
		}
	}
}
