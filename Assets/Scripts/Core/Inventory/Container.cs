using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Container
{
    public event Action OnContentsChanged;

    [SerializeField] private int _maxItems;
    [SerializeField] private List<InventoryItemInstance> _items;

    [SerializeField] private string _name;
    [SerializeField] private bool _showName = false;

    public List<InventoryItemInstance> Items { get => _items; }

    public string Name { get => _name; }
    public bool ShowName { get => _showName; }

    public bool CanAddItem()
    {
        return _items.Count < _maxItems;
    }

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
        OnContentsChanged?.Invoke();
    }

    private void GiveItem(InventoryItemInstance item)
    {
        if (_items.Contains(item))
        {
            throw new Exception("Item is already here!");
        }

        _items.Add(item);
        OnContentsChanged?.Invoke();
    }

    public void RemoveItem(InventoryItemInstance item)
    {
        _items.Remove(item);
        OnContentsChanged?.Invoke();
    }

    public void RemoveItemOfType(Type type)
    {
        InventoryItemInstance item = _items.FirstOrDefault(i => i.Item.GetType() == type);

        if (item != null)
        {
            _items.Remove(item);
            OnContentsChanged?.Invoke();
        }
    }

    public void Update()
    {
        foreach (InventoryItemInstance item in _items)
        {
            item.Update();
        }
    }
}