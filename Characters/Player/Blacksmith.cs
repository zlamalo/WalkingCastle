using Godot;
using System;

public partial class Blacksmith : CharacterBody2D
{
	private AnimationPlayer bodyAnimationPlayer;
	private AnimationPlayer armsAnimationPlayer;
	private AnimatedSprite2D blacksmithSprite;
	private Node2D arms;
	private Node2D rightArm;
	private bool lookingRight = true;

	public const float Speed = 150.0f;

	public override void _Ready()
	{
		bodyAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		blacksmithSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		arms = GetNode<Node2D>("Arms");
		armsAnimationPlayer = arms.GetNode<AnimationPlayer>("AnimationPlayer");
		rightArm = arms.GetNode<Node2D>("RightArm");
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
			}
			else
			{
				rightArm.ZIndex = blacksmithSprite.ZIndex - 1;
			}
		}
	}
}
