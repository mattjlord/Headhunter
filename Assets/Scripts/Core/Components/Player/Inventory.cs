using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Container _container;
    [SerializeField] private float _carryingCapacity = 50f;

    public Container Container => _container;

    public bool CanTakeItem(InventoryItem item) => _container.TotalWeight + item.Weight < _carryingCapacity;
}
