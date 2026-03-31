using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot
{
    Head,
    Torso,
    Legs,
    Back
}

public abstract class AEquippable : InventoryItem
{
    public EquipmentSlot equipmentSlot;

    public abstract void OnEquip(PlayerOrganism organism);
    public abstract void OnUnequip(PlayerOrganism organism);

    public override List<ItemInteractionType> GetInteractionOptions() => new List<ItemInteractionType>() { ItemInteractionType.Equip, ItemInteractionType.Discard };
}