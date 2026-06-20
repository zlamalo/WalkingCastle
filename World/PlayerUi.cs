using Godot;
using System;

public partial class PlayerUi : CanvasLayer
{
	private PackedScene playerinventoryScene = GD.Load<PackedScene>("res://UserInterface/PlayerUi/PlayerInventoryUi.tscn");

	private PlayerInventoryUi playerInventoryUi;
	private bool isPlayerInventoryOpen = false;

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (Input.IsActionJustPressed("OpenInventory"))
		{
			if (isPlayerInventoryOpen)
			{
				playerInventoryUi?.QueueFree();
			}
			else
			{
				playerInventoryUi = playerinventoryScene.Instantiate<PlayerInventoryUi>();
				AddChild(playerInventoryUi);
			}

			isPlayerInventoryOpen = !isPlayerInventoryOpen;
			GetTree().Paused = isPlayerInventoryOpen;
			return;
		}
	}
}
