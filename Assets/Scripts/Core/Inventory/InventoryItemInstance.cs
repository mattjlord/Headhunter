using System;
using UnityEngine;

[Serializable]
public class InventoryItemInstance
{
    [SerializeField] private AInventoryItem _item;
    private float _durability;
    private EquipmentSlot? _equipmentSlot = null;

    public InventoryItemInstance(AInventoryItem item)
    {
        _item = item;
    }

    public AInventoryItem Item { get => _item; }

    public EquipmentSlot? EquipmentSlot { get => _equipmentSlot; set => _equipmentSlot = value; }

    public void DecreaseDurability(float amount)
    {
        if (amount < 0) { return;  }
        float result = _durability - amount;
        if (result > 0)
        {
            _durability = result;
        }
        else
        {
            _durability = 0;
            _item = _item.GetPerishedVersion();
        }
    }
}