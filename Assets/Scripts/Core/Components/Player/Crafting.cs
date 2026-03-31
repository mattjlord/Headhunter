using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] private Container _inputs;
    [SerializeField] private Container _outputs;

    public Container Inputs
    {
        get => _inputs;
    }
    public Container Outputs => _outputs;

    public bool CanCraft
    {
        get
        {
            List<CraftingMaterial> materials = new List<CraftingMaterial>();
            foreach (InventoryItemInstance instance in _inputs.Items)
                materials.Add(instance.Item as CraftingMaterial);
            return _outputs.Items.Count == 0 && CraftingLibrary.CanCraft(materials);
        }
    }

    public void Craft()
    {
        List<CraftingMaterial> materials = new List<CraftingMaterial>();
        foreach (InventoryItemInstance instance in _inputs.Items)
            materials.Add(instance.Item as CraftingMaterial);
        InventoryItem output = CraftingLibrary.GetCraftingOutput(materials);
        InventoryItemInstance outputInstance = new InventoryItemInstance(output);
        _inputs.Items.Clear();
        _outputs.Items.Add(outputInstance);
    }
}
