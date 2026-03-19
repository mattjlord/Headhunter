using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShelter : WorldObject
{
    [SerializeField] private float _recoveryPerMinute;

    public float RecoveryPerMinute { get => _recoveryPerMinute; }

    public override void OnInteraction(PlayerController playerController)
    {
        playerController.RestInShelter(this);
    }
}
