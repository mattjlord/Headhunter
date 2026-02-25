using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : ContainerUI
{
    [SerializeField] private Inventory _inventory;

    protected override void Start()
    {
        base.Start();
        Container = _inventory.Container;
    }
}
