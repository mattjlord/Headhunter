using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float _baseDamage;
    [SerializeField] private float _variance;
    // TODO: Bleeding

    public void AttackOrganism(Organism organism)
    {
        float damage = GenerateDamage();
        organism.Vitals.GetVital(VitalType.Injury).IncreaseValue(damage);
    }

    private float GenerateDamage()
    {
        float modifier = Random.Range(-_variance, _variance);
        return _baseDamage + modifier;
    }
}
