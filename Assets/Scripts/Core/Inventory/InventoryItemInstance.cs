using System;
using UnityEngine;

[Serializable]
public class InventoryItemInstance
{
    [SerializeField] private AInventoryItem _item;
    private float _durability;

    public InventoryItemInstance(AInventoryItem item)
    {
        _item = item;
    }

    public AInventoryItem Item { get => _item; }
}