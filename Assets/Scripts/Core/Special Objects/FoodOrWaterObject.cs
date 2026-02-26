using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrWaterObject : WorldObject
{
    [SerializeField] protected float hungerImpact;
    [SerializeField] protected float thirstImpact;
    [SerializeField] protected int uses = -1; // -1 indicates an infinite resource

    public void ConsumeThis(Organism organism)
    {
        if (uses == 0)
            return;

        organism.Vitals.GetVital(VitalType.Hunger).DecreaseValue(hungerImpact);
        organism.Vitals.GetVital(VitalType.Thirst).DecreaseValue(thirstImpact);

        if (uses > 0)
        {
            OnConsumeOnce();
            uses--;
        }
    }

    public bool CanConsume()
    {
        return uses != 0;
    }

    protected virtual void OnConsumeOnce() { }
}
