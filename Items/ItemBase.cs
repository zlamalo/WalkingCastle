using Godot;
using System;

public partial class ItemBase : Area2D
{
	private Sprite2D sprite;
	private ShaderMaterial shaderMaterial;

	private PackedScene textPanelScene = GD.Load<PackedScene>("res://UserInterface/TextPanelUi.tscn");

	[Export]
	public Item Item { get; set; }

	public override void _Ready()
	{
		if (Item == null)
		{
			GD.PrintErr("ItemBase has no item assigned.");
			return;
		}

		sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Texture = Item?.ItemResource?.Icon;

		shaderMaterial = sprite.Material as ShaderMaterial;
	}

	private void Highlight(bool highlight)
	{
		shaderMaterial?.SetShaderParameter("outline_enabled", highlight);
	}
}
