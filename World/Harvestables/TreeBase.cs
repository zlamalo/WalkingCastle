using Godot;
using System;

public partial class TreeBase : StaticBody2D
{
	private readonly ItemService itemService = GlobalServices.Instance.ItemService;
	private AnimationPlayer animationPlayer;
	private Area2D hitboxArea;
	private CollisionShape2D treeCollision;
	private int HitPoints;

	[Export]
	public Harvestable HarvestableResource;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		hitboxArea = GetNode<Area2D>("Area2D");
		treeCollision = GetNode<CollisionShape2D>("CollisionShape2D");
		HitPoints = HarvestableResource.HitPoints;
	}

	public void OnAreaEntered(Area2D area)
	{
		if (area is ItemArea item)
		{
			if (item.Item.ItemResource is Tool tool)
			{
				if (tool.Type == HarvestableResource.HarvestableBy)
				{
					OnHit();
				}
			}
		}
	}

	private void OnHit()
	{
		animationPlayer.Play("OnHit");
		HitPoints--;
		if (HitPoints <= 0)
		{
			OnHarvested();
		}
	}

	private void OnHarvested()
	{
		var drops = itemService.GetLootInstances(HarvestableResource.Loot);
		foreach (var drop in drops)
		{
			drop.GlobalPosition = treeCollision.GlobalPosition;
			GlobalNodes.Instance.GameWorld.CallDeferred(Node.MethodName.AddChild, drop);
		}

		CallDeferred(Node.MethodName.QueueFree);
	}
}
