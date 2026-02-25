using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carcass : FoodOrWaterObject
{
    [SerializeField] private Container _scavengeableParts;

    public Container ScavengeableParts => _scavengeableParts;

    public override void OnInteraction(PlayerController playerController)
    {
        playerController.OpenContainer(_scavengeableParts);
    }
}
