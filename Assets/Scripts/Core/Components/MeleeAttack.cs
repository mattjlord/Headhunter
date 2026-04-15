using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float _baseDamage;
    [SerializeField] private float _variance;
    [SerializeField] private DamageType _damageType = DamageType.Physical;
    [SerializeField] private Transform _impactPoint;
    // TODO: Bleeding

    public void AttackOrganism(Organism target, Organism attacker)
    {
        float damage = GenerateDamage();
        target.TakeDamage(damage, _damageType, attacker.Position, _impactPoint.position);
    }

    private float GenerateDamage()
    {
        float modifier = Random.Range(-_variance, _variance);
        return _baseDamage + modifier;
    }
}
