using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<float> OnEncumbranceChanged;

    [SerializeField] private Container _container;
    [SerializeField] private float _carryingCapacity = 50f;

    // If equipment ever becomes more complex, feel free to refactor this
    [SerializeField] private InventoryItemInstance? _head = null;
    [SerializeField] private InventoryItemInstance? _torso = null;
    [SerializeField] private InventoryItemInstance? _legs = null;
    [SerializeField] private InventoryItemInstance? _back = null;

    private void Start()
    {
        _container.OnContentsChanged += OnContainerChanged;
    }

    public Container Container => _container;

    public bool CanTakeItem(InventoryItemInstance item) => _container.TotalWeight + item.Item.Weight < _carryingCapacity;

    public void ProcessItemInteraction(InventoryItemInstance item, ItemInteractionType interaction, PlayerOrganism organism)
    {
        Debug.Log("Interaction...");
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
                if (item.EquipmentSlot == null)
                    EquipItem(item, organism);
                else
                    UnequipItem(item, organism);
                return;
        }
    }

    private void EquipItem(InventoryItemInstance item, PlayerOrganism organism)
    {
        Debug.Log("Equip");
        AEquippable equippable = item.Item as AEquippable;

        AEquippable? lastEquipped = null;

        switch (equippable.equipmentSlot)
        {
            case EquipmentSlot.Head:
                lastEquipped = _head?.Item as AEquippable;
                if (_head != null)
                    _head.EquipmentSlot = null;
                _head = item;
                item.EquipmentSlot = EquipmentSlot.Head;
                break;
            case EquipmentSlot.Torso:
                lastEquipped = _torso?.Item as AEquippable;
                if (_torso != null)
                    _torso.EquipmentSlot = null;
                _torso = item;
                item.EquipmentSlot = EquipmentSlot.Torso;
                break;
            case EquipmentSlot.Legs:
                lastEquipped = _legs?.Item as AEquippable;
                if (_legs != null)
                    _legs.EquipmentSlot = null;
                _legs = item;
                item.EquipmentSlot = EquipmentSlot.Legs;
                break;
            case EquipmentSlot.Back:
                lastEquipped = _back?.Item as AEquippable;
                if (_back != null)
                    _back.EquipmentSlot = null;
                _back = item;
                item.EquipmentSlot = EquipmentSlot.Back;
                break;
        }

        lastEquipped?.OnUnequip(organism);
        equippable.OnEquip(organism);
    }

    private void UnequipItem(InventoryItemInstance item, PlayerOrganism organism)
    {
        Debug.Log("Unequip");
        AEquippable equippable = item.Item as AEquippable;
        equippable.OnUnequip(organism);

        switch (item.EquipmentSlot)  
        {
            case EquipmentSlot.Head:
                _head.EquipmentSlot = null;
                _head = null;
                break;
            case EquipmentSlot.Torso:
                _torso.EquipmentSlot = null;
                _torso = null;
                break;
            case EquipmentSlot.Legs:
                _legs.EquipmentSlot = null;
                _legs = null;
                break;
            case EquipmentSlot.Back:
                _back.EquipmentSlot = null;
                _back = null;
                break;
        }
    }

    private void OnContainerChanged()
    {
        float encumbrance = _container.TotalWeight / _carryingCapacity;
        OnEncumbranceChanged?.Invoke(encumbrance);
    }
}
