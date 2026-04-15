using UnityEngine;

public enum CraftingMaterialType
{
    Lead,
    GlowPowder
}

[CreateAssetMenu(fileName = "New Crafting Material", menuName = "Inventory Item/Crafting Material")]
public class CraftingMaterial : InventoryItem
{
    public CraftingMaterialType type;

    public override string GetTypeName() => "Crafting Material";
}
