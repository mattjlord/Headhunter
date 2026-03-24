using System;
using UnityEngine;

[Serializable]
public class InventoryItemInstance
{
    [SerializeField] private AInventoryItem _item;
    [SerializeField] private float _durability = 100f;
    private EquipmentSlot? _equipmentSlot = null;

    public InventoryItemInstance(AInventoryItem item)
    {
        _item = item;
    }

    public AInventoryItem Item { get => _item; }

    public EquipmentSlot? EquipmentSlot { get => _equipmentSlot; set => _equipmentSlot = value; }

    public float Durability { get => _durability; }

    public void Update()
    {
        float decayRate = 0f;

        if (_item.GetType() == typeof(Consumable))
        {
            Consumable consumable = _item as Consumable;
            decayRate = consumable.DecayRate;
        }

        if (decayRate > 0)
        {
            float decayThisFrame = decayRate * (TimeManagement.GameDeltaTime / 60f);
            DecreaseDurability(decayThisFrame);
        }
    }

    private void DecreaseDurability(float amount)
    {
        if (amount < 0) { return; }
        float result = _durability - amount;
        if (result > 0)
        {
            _durability = result;
        }
        else
        {
            _item = _item.GetPerishedVersion();
            _durability = 100;
        }
    }
}