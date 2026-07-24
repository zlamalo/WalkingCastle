using Godot;
using System;
using System.Threading.Tasks;

public partial class HarvestableBase : StaticBody2D
{
	private readonly ItemService itemService = GlobalServices.Instance.ItemService;
	private readonly FloatingTextService floatingTextService = GlobalServices.Instance.FloatingTextService;

	private Sprite2D harvestableSprite;
	private Image harvestableImage;
	private AnimationPlayer animationPlayer;
	private Area2D hitboxArea;
	private CollisionShape2D harvestableCollision;
	private Player playerInProximity = null;

	private int HitPoints;

	[Export]
	public Harvestable HarvestableResource;

	public override void _Ready()
	{
		if (HarvestableResource == null)
		{
			GD.PrintErr("HarvestableResource is null in HarvestableBase");
			return;
		}

		harvestableSprite = GetNode<Sprite2D>("HarvestableSprite");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		hitboxArea = GetNode<Area2D>("%HarvestableHitboxArea");
		harvestableCollision = GetNode<CollisionShape2D>("HarvestableCollision");
		HitPoints = HarvestableResource.HitPoints;

		harvestableSprite.Texture = HarvestableResource.HarvestableTexture;
		harvestableImage = harvestableSprite.Texture.GetImage();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (playerInProximity == null)
			return;

		if (playerInProximity.GlobalPosition.Y < harvestableCollision.GlobalPosition.Y)
		{
			Modulate = new Color(1, 1, 1, 0.5f);
		}
		else
		{
			Modulate = new Color(1, 1, 1, 1);
		}
	}

	public void OnAreaEntered(Area2D area)
	{
		if (area.GetParent() is ToolBase toolBase)
		{
			if (toolBase.Tool.Type != HarvestableResource.HarvestableBy)
			{
				floatingTextService.ShowText(GlobalPosition, "This tool cannot harvest this resource.", Colors.Gray);
				return;
			}
			if (toolBase.Tool.ToolPower < HarvestableResource.RequiredToolPower)
			{
				floatingTextService.ShowText(GlobalPosition, "Too hard", Colors.Gray);
				return;
			}
			OnHit();
		}
	}

	public void OnBodyEntered(Node2D node)
	{
		if (node is Player player)
		{
			playerInProximity = player;
		}
	}

	public void OnBodyExited(Node2D node)
	{
		if (node is Player player && playerInProximity == player)
		{
			playerInProximity = null;
			Modulate = new Color(1, 1, 1, 1);
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
			drop.GlobalPosition = harvestableCollision.GlobalPosition;
			GlobalNodes.Instance.GameWorld.CallDeferred(Node.MethodName.AddChild, drop);
		}

		CallDeferred(Node.MethodName.QueueFree);
	}

	public bool IsPlayerInsideVisibleSprite(Sprite2D sprite, Vector2 globalPos)
	{
		if (sprite.Texture == null)
			return false;

		Vector2 local = sprite.ToLocal(globalPos);

		Image image = sprite.Texture.GetImage();
		Vector2 size = image.GetSize();

		Vector2 pixel = local + size / 2f;

		int x = Mathf.FloorToInt(pixel.X);
		int y = Mathf.FloorToInt(pixel.Y);

		if (x < 0 || y < 0 || x >= size.X || y >= size.Y)
			return false;

		Color color = image.GetPixel(x, y);

		return color.A > 0.1f;
	}

}
