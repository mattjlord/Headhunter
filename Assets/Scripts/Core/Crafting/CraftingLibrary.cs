using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingLibrary : MonoBehaviour
{
    [SerializeField] private List<SerializableRecipe> _serializableRecipes;
    private static Dictionary<CraftingIngredients, InventoryItem> _allRecipes;

    private void Awake()
    {
        _allRecipes = new Dictionary<CraftingIngredients, InventoryItem>();
        foreach (SerializableRecipe recipe in _serializableRecipes)
        {
            recipe.AddToRecipes(_allRecipes);
        }
    }

    public static InventoryItem GetCraftingOutput(List<CraftingMaterial> inputs)
    {
        CraftingIngredients ingredients = new CraftingIngredients(inputs);
        return _allRecipes[ingredients];
    }

    public static bool CanCraft(List<CraftingMaterial> inputs)
    {
        CraftingIngredients ingredients = new CraftingIngredients(inputs);
        return _allRecipes.ContainsKey(ingredients);
    }
}
