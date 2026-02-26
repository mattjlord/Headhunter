using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Container _container;
    [SerializeField] private float _carryingCapacity = 50f;

    public Container Container => _container;

    public bool CanTakeItem(InventoryItemInstance item) => _container.TotalWeight + item.Item.Weight < _carryingCapacity;

    public void ProcessItemInteraction(InventoryItemInstance item, ItemInteractionType interaction, Organism organism)
    {
        switch (interaction)
        {
            case ItemInteractionType.Discard:
                _container.RemoveItem(item);
                return;
            case ItemInteractionType.Consume:
                Consumable consumable = item.Item as Consumable;
                if (consumable != null)
                    consumable.ImpactOrganism(organism);
                _container.RemoveItem(item);
                return;
            case ItemInteractionType.Equip:
                // TODO: Equip item
                return;
        }
    }
}
