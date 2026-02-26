using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carcass : FoodOrWaterObject
{
    [SerializeField] private Container _scavengeableParts;

    public Container ScavengeableParts => _scavengeableParts;

    private void Awake()
    {
        int count = 0;
        foreach(InventoryItemInstance item in _scavengeableParts.Items)
        {
            if (item.Item.GetType() == typeof(Consumable))
            {
                count++;
            }
        }
        uses = count;
    }

    public override void OnInteraction(PlayerController playerController)
    {
        playerController.OpenContainer(_scavengeableParts);
    }

    protected override void OnConsumeOnce()
    {
        _scavengeableParts.RemoveItemOfType(typeof(Consumable));
    }
}
