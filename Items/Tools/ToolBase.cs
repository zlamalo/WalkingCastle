using Godot;
using System;
using System.ComponentModel;

public partial class ToolBase : Node2D
{
	private Area2D toolHitbox;
	private CollisionShape2D toolHitboxCollisionShape;
	private Sprite2D toolSprite;

	public Tool Tool;

	public override void _Ready()
	{
		toolHitbox = GetNode<Area2D>("%ToolHitbox");
		toolHitboxCollisionShape = GetNode<CollisionShape2D>("%ToolHitbox/CollisionShape2D");
		toolSprite = GetNode<Sprite2D>("%ToolSprite");

		toolHitboxCollisionShape.Disabled = true;
		toolSprite.Texture = Tool.Icon;
	}

	public void ToggleHitbox(bool active)
	{
		toolHitboxCollisionShape.Disabled = !active;
	}
}
