using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : ContainerUI
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Image _headItem;
    [SerializeField] private Image _torsoItem;
    [SerializeField] private Image _legsItem;
    [SerializeField] private Image _backItem;

    protected override void Start()
    {
        base.Start();
        Container = _inventory.Container;
    }

    protected override void Update()
    {
        base.Update();
        DisplayEquipment();
    }

    private void DisplayEquipment()
    {
        InventoryItemInstance headItem = _inventory.Head;
        InventoryItemInstance torsoItem = _inventory.Torso;
        InventoryItemInstance legsItem = _inventory.Legs;
        InventoryItemInstance backItem = _inventory.Back;

        _headItem.enabled = headItem != null;
        _torsoItem.enabled = torsoItem != null;
        _legsItem.enabled = legsItem != null;
        _backItem.enabled = backItem != null;

        if (headItem != null)
            _headItem.sprite = headItem.Item?.Image;
        if (torsoItem != null)
            _torsoItem.sprite = torsoItem.Item?.Image;
        if (legsItem != null)
            _legsItem.sprite = legsItem.Item?.Image;
        if (backItem != null)
            _backItem.sprite = backItem.Item?.Image;
    }
}
