using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableRecipe
{
    [SerializeField] private List<CraftingMaterial> _inputs;
    [SerializeField] private InventoryItem _output;

    public void AddToRecipes(Dictionary<CraftingIngredients, InventoryItem> recipes)
    {
        CraftingIngredients ingredients = new CraftingIngredients(_inputs);
        recipes.Add(ingredients, _output);
    }
}