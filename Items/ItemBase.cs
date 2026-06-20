using Godot;
using System;

public partial class ItemBase : Area2D
{
	private Sprite2D sprite;
	private ShaderMaterial shaderMaterial;
	private Node2D attractorNode = null;

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

	public override void _Process(double delta)
	{
		if (attractorNode != null)
		{
			GlobalPosition = GlobalPosition.MoveToward(attractorNode.GlobalPosition, 100 * (float)delta);
		}
	}

	public void SetAttractorNode(Node2D node)
	{
		attractorNode = node;
	}

	private void Highlight(bool highlight)
	{
		shaderMaterial?.SetShaderParameter("outline_enabled", highlight);
	}
}
