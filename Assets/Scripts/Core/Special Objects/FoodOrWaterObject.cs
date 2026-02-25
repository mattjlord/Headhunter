using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrWaterObject : WorldObject
{
    [SerializeField] private float _hungerImpact;
    [SerializeField] private float _thirstImpact;
    [SerializeField] private int _uses = -1; // -1 indicates an infinite resource

    public void ConsumeThis(Organism organism)
    {
        if (_uses == 0)
            return;

        organism.Vitals.GetVital(VitalType.Hunger).DecreaseValue(_hungerImpact);
        organism.Vitals.GetVital(VitalType.Thirst).DecreaseValue(_thirstImpact);

        if (_uses > 0)
            _uses--;
    }

    public bool CanConsume()
    {
        return _uses != 0;
    }
}
