using System;
using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	private AnimationPlayer bodyAnimationPlayer;
	private AnimationPlayer armsAnimationPlayer;
	private AnimatedSprite2D blacksmithSprite;
	private Node2D arms;
	private Node2D rightArm;
	private Node2D leftArm;
	private Area2D pickupRange;
	private bool lookingRight = true;

	[Export]
	private Array<Item> starterItems = [];

	public Inventory Inventory { get; private set; }
	public const float Speed = 150.0f;

	public override void _Ready()
	{
		bodyAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		blacksmithSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		arms = GetNode<Node2D>("Arms");
		armsAnimationPlayer = arms.GetNode<AnimationPlayer>("AnimationPlayer");
		rightArm = arms.GetNode<Node2D>("RightArm");
		leftArm = arms.GetNode<Node2D>("LeftArm");
		pickupRange = GetNode<Area2D>("AttractRange");

		Inventory = GetNode<Inventory>("Inventory");
		foreach (var item in starterItems)
		{
			Inventory.AddItem(item);
		}
		Inventory.ToolbarSelectedItemChanged += OnToolbarSelectedItemChanged;
		OnToolbarSelectedItemChanged(Inventory.selectedItemIndex);

		GlobalNodes.Instance.SetPlayer(this);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (Input.IsActionPressed("Interact"))
		{
			foreach (var area in pickupRange.GetOverlappingAreas())
			{
				if (area is ItemBase itemBase)
				{
					Inventory.AddItem(itemBase.Item);
					itemBase.QueueFree();
				}
			}
			pickupRange.GetOverlappingAreas();
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("PlayerLeft", "PlayerRight", "PlayerUp", "PlayerDown");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		ProcessAnimation();

		if (Input.IsActionPressed("UseItem"))
		{
			armsAnimationPlayer.Play("Swing");
		}
	}

	private void ProcessAnimation()
	{

		if (Velocity.X != 0 || Velocity.Y != 0)
		{
			bodyAnimationPlayer.Play("Walk");
		}
		else
		{
			bodyAnimationPlayer.Play("Idle");
		}

		Vector2 directionToMouse = GetGlobalMousePosition() - blacksmithSprite.GlobalPosition;
		var mouseOnRightSide = directionToMouse.X > 0;
		// Character turns
		if (lookingRight ^ mouseOnRightSide)
		{
			lookingRight = mouseOnRightSide;
			blacksmithSprite.FlipH = !lookingRight;
			arms.ApplyScale(new Vector2(-1, 1));
			if (lookingRight)
			{
				rightArm.ZIndex = blacksmithSprite.ZIndex + 1;
				leftArm.ZIndex = blacksmithSprite.ZIndex - 1;

			}
			else
			{
				rightArm.ZIndex = blacksmithSprite.ZIndex - 1;
				leftArm.ZIndex = blacksmithSprite.ZIndex + 1;
			}
		}
	}

	// Freeky ahh code needs refactor
	private void OnToolbarSelectedItemChanged(int selectedIndex)
	{
		var itemPlaceholder = rightArm.GetNode<Node2D>("ItemPlaceholder");
		itemPlaceholder.GetChildOrNull<Node2D>(0)?.QueueFree();
		// reset hitbox to default (fist)
		var toolHitbox = rightArm.GetNode<ItemArea>("Hitbox");
		var hitboxCollisionShape = rightArm.GetNode<CollisionShape2D>("Hitbox/CollisionShape2D");
		hitboxCollisionShape.Position = Vector2.Zero;
		hitboxCollisionShape.Shape = new CircleShape2D { Radius = 0.8f };

		var item = Inventory.Items[selectedIndex];
		if (item == null || item.ItemResource?.ItemScene == null || item.ItemResource is not Tool)
			return;
		var itemInstance = Inventory.Items[selectedIndex]?.ItemResource?.ItemScene.Instantiate();
		if (itemInstance != null)
		{
			toolHitbox.Item = item;

			itemPlaceholder.AddChild(itemInstance);

			if (Inventory.Items[selectedIndex]?.ItemResource is Tool tool)
			{
				var newCollisionShape = itemInstance.GetNode<CollisionShape2D>("%ItemCollisionArea");
				hitboxCollisionShape.Position = newCollisionShape.Position;
				hitboxCollisionShape.Shape = newCollisionShape.Shape;
			}
		}
	}

	private void OnItemInAttrackRange(Area2D item)
	{
		if (!GlobalServices.Instance.GameSettings.AutoPickup)
		{
			GlobalServices.Instance.TooltipService.ShowTooltip(this, new Vector2(0, -16), "Press F to pickup items");
		}
		else
		{
			if (item is ItemBase itemBase)
			{
				itemBase.SetAttractorNode(this);
			}
		}

	}

	private void OnItemInPickupRange(Area2D itemNode)
	{
		if (itemNode is ItemBase itemBase)
		{
			var item = itemBase.Item;
			var quantity = item is StackableItem stackable
				? stackable.Quantity
				: 1;

			GlobalServices.Instance.InformationLogService.DisplayInformation(
				$"Picked up {quantity} {item.ItemResource.Name}");
			Inventory.AddItem(item);
			itemBase.QueueFree();
		}
	}
}
