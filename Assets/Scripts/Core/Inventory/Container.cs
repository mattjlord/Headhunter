using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Container
{
    [SerializeField] private List<InventoryItemInstance> _items;

    public List<InventoryItemInstance> Items { get { return _items; } }

    public float TotalWeight
    {
        get
        {
            float total = 0;
            foreach (InventoryItemInstance instance in _items)
            {
                total += instance.Item.Weight;
            }
            return total;
        }
    }

    public void TakeItem(InventoryItemInstance item, Container receiver)
    {
        if (!_items.Contains(item))
        {
            throw new Exception("Tried to remove an item that isn't there!");
        }

        receiver.GiveItem(item);
        _items.Remove(item);
    }

    public void GiveItem(InventoryItemInstance item)
    {
        if (_items.Contains(item))
        {
            throw new Exception("Item is already here!");
        }

        _items.Add(item);
    }

    public void RemoveItemOfType(Type type)
    {
        InventoryItemInstance item = _items.FirstOrDefault(i => i.Item.GetType() == type);

        if (item != null)
            _items.Remove(item);
    }
}