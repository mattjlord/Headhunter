using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Container
{
    [SerializeField] private List<InventoryItem> _items;

    public List<InventoryItem> Items { get { return _items; } }

    public float TotalWeight
    {
        get
        {
            float total = 0;
            foreach (InventoryItem item in _items)
            {
                total += item.Weight;
            }
            return total;
        }
    }

    public void TakeItem(InventoryItem item, Container receiver)
    {
        if (!_items.Contains(item))
        {
            throw new Exception("Tried to remove an item that isn't there!");
        }

        receiver.GiveItem(item);
        _items.Remove(item);
    }

    public void GiveItem(InventoryItem item)
    {
        if (_items.Contains(item))
        {
            throw new Exception("Item is already here!");
        }

        _items.Add(item);
    }
}