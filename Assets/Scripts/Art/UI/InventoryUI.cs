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
    [SerializeField] private Sprite _headEmptyIcon;
    [SerializeField] private Sprite _torsoEmptyIcon;
    [SerializeField] private Sprite _legsEmptyIcon;
    [SerializeField] private Sprite _backEmptyIcon;

    [SerializeField] private List<Image> _equipmentFrames;


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

    protected override void EnabledExtension(bool value)
    {
        foreach (Image frame in _equipmentFrames)
            frame.enabled = value;
        _headItem.enabled = value;
        _torsoItem.enabled = value;
        _legsItem.enabled = value;
        _backItem.enabled = value;
    }

    private void DisplayEquipment()
    {
        InventoryItemInstance headItem = _inventory.Head;
        InventoryItemInstance torsoItem = _inventory.Torso;
        InventoryItemInstance legsItem = _inventory.Legs;
        InventoryItemInstance backItem = _inventory.Back;

        if (headItem != null)
            _headItem.sprite = headItem.Item?.Image;
        else
            _headItem.sprite = _headEmptyIcon;
        if (torsoItem != null)
            _torsoItem.sprite = torsoItem.Item?.Image;
        else
            _torsoItem.sprite = _torsoEmptyIcon;
        if (legsItem != null)
            _legsItem.sprite = legsItem.Item?.Image;
        else
            _legsItem.sprite = _legsEmptyIcon;
        if (backItem != null)
            _backItem.sprite = backItem.Item?.Image;
        else
            _backItem.sprite = _backEmptyIcon;
    }
}
