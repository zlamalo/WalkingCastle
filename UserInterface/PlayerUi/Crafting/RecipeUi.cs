using Godot;
using System;
using System.Linq;

public partial class RecipeUi : Control
{
	private PackedScene recipeItemSlotScene = GD.Load<PackedScene>("res://UserInterface/PlayerUi/Crafting/RecipeItemSlotUi.tscn");

	private GridContainer ingredientsContainer;
	private RecipeItemSlotUi resultSlot;

	[Signal]
	public delegate void RecipeSelectedEventHandler(Recipe recipe);

	public Recipe Recipe { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ingredientsContainer = GetNode<GridContainer>("%IngredientsContainer");
		resultSlot = GetNode<RecipeItemSlotUi>("%ResultItem");

		GlobalNodes.Instance.Player.Inventory.InventoryChanged += OnInventoryChanged;
	}

	public override void _ExitTree()
	{
		GlobalNodes.Instance.Player.Inventory.InventoryChanged -= OnInventoryChanged;
	}

	public void SetRecipe(Recipe recipe)
	{
		Recipe = recipe;
		ingredientsContainer.GetChildren()?.ToList().ForEach(x => x.QueueFree());

		foreach (var ingredient in recipe.Ingredients)
		{
			var slot = recipeItemSlotScene.Instantiate<RecipeItemSlotUi>();
			ingredientsContainer.AddChild(slot);

			if (ingredient is StackableItem stackable)
			{
				var itemCount = GlobalNodes.Instance.Player.Inventory.ContainsItemAmount(ingredient);
				var quantity = stackable.Quantity;
				var textColor = itemCount >= quantity ? Colors.Green : Colors.Red;
				slot.UpdateItem(ingredient, $"{itemCount}/{quantity}", textColor);
			}
			else
			{
				slot.UpdateItem(ingredient);
			}
		}
		resultSlot.UpdateItem(recipe.Result);
	}

	public void Highlight(bool highlighted)
	{
		SelfModulate = highlighted ? new Color(1.353f, 1.353f, 1.353f) : Colors.White;
	}

	public void OnFocusEntered()
	{
		EmitSignal(SignalName.RecipeSelected, Recipe);
	}

	/// <summary>
	/// Redraws the recipe with current inventory state
	/// </summary>
	/// <param name="newInventory"></param>
	private void OnInventoryChanged(Godot.Collections.Array<Item> newInventory)
	{
		SetRecipe(Recipe);
	}
}
