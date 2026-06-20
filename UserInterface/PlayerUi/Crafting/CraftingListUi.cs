using Godot;
using Godot.Collections;
using System;

public partial class CraftingListUi : Control
{
	private PackedScene recipeScene = GD.Load<PackedScene>("res://UserInterface/PlayerUi/Crafting/RecipeUi.tscn");

	private VBoxContainer recipesList;
	private Array<RecipeUi> recipeUis = [];

	private int selectedRecipeIndex = -1;

	private Array<Recipe> Recipes;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Recipes = GlobalServices.Instance.RecipeService.GetAllRecipes();

		recipesList = GetNode<VBoxContainer>("%RecipesList");
		foreach (var recipe in Recipes)
		{
			var recipeUi = recipeScene.Instantiate<RecipeUi>();
			recipeUi.RecipeSelected += OnRecipeSelected;
			recipesList.AddChild(recipeUi);
			recipeUi.SetRecipe(recipe);
			recipeUis.Add(recipeUi);
		}
		//recipeUis[0].Highlight(true);
		if (Recipes.Count > 0)
			OnRecipeSelected(Recipes[0]);
	}

	public void CraftSelectedRecipe()
	{
		GlobalServices.Instance.RecipeService.CraftRecipe(Recipes[selectedRecipeIndex]);
	}

	private void OnRecipeSelected(Recipe recipe)
	{
		var index = Recipes.IndexOf(recipe);
		if (index != -1)
		{
			if (selectedRecipeIndex != -1 && selectedRecipeIndex < recipeUis.Count)
			{
				recipeUis[selectedRecipeIndex].Highlight(false);
			}
			selectedRecipeIndex = index;
			recipeUis[selectedRecipeIndex].Highlight(true);
		}
	}
}
