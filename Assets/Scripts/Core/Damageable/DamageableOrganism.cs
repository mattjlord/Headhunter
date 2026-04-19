using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableOrganism : MonoBehaviour, IDamageable
{
    [SerializeField] private Organism organism;

    public void TakeDamage(float damage, DamageType damageType, Vector2? worldSource, Vector3? impactPoint, Action<Organism> killEvent = null)
    {
        organism.TakeDamage(damage, damageType, worldSource, impactPoint, killEvent);
    }

    public bool Enabled() => true;
}
