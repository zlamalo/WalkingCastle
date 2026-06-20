using System;
using Godot;
using Godot.Collections;

public class RecipeService
{
    private Inventory inventory => GlobalNodes.Instance.Player.Inventory;
    private Array<Recipe> recipes = [];

    public Array<Recipe> GetAllRecipes()
    {
        if (recipes.Count > 0)
        {
            return recipes;
        }
        var loadedRecipes = GlobalServices.Instance.ResourceLoaderService.LoadResourcesFromPath<Recipe>("res://Items/Recipes/Resources");
        recipes = [.. loadedRecipes];
        return recipes;
    }

    public void CraftRecipe(Recipe recipe)
    {
        if (CanCraft(recipe, inventory))
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                inventory.RemoveItem(ingredient.Duplicate() as Item);
            }

            inventory.AddItem(recipe.Result);
        }
    }

    /// <summary>
    /// Checks if recipe is craftable with the given inventory
    /// </summary>
    /// <param name="recipe"></param>
    /// <param name="inventory"></param>
    /// <returns></returns>
    private bool CanCraft(Recipe recipe, Inventory inventory)
    {
        foreach (var ingredient in recipe.Ingredients)
        {
            if (ingredient is StackableItem stackable)
            {
                if (inventory.ContainsItemAmount(ingredient) < stackable.Quantity)
                {
                    return false;
                }
            }
            else if (inventory.ContainsItemAmount(ingredient) < 1)
            {
                return false;
            }
        }
        return true;
    }
}