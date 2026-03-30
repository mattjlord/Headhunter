using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory Item/Consumable")]
public class Consumable : AInventoryItem
{
    public bool Perishable;
    public bool Cookable;
    // TODO: Implement decay
    public float DecayRate; // Per minute of game time
    public Consumable? PerishedVersion;
    public Consumable? CookedVersion;
    public float HungerImpact;
    public float ThirstImpact;
    public float ExhaustionImpact;
    public float HeatImpact;
    public float InjuryImpact;
    public float ToxicityImpact;

    public void ImpactOrganism(Organism organism)
    {
        Vitals vitals = organism.Vitals;
        ImpactVital(vitals, VitalType.Hunger, HungerImpact);
        ImpactVital(vitals, VitalType.Thirst, ThirstImpact);
        ImpactVital(vitals, VitalType.Exhaustion, ExhaustionImpact);
        ImpactVital(vitals, VitalType.Heat, HeatImpact);
        ImpactVital(vitals, VitalType.Injury, InjuryImpact);
        ImpactVital(vitals, VitalType.Toxicity, ToxicityImpact);
    }

    private void ImpactVital(Vitals vitals, VitalType type, float value)
    {
        if (value > 0)
        {
            vitals.GetVital(type).IncreaseValue(value);
        }
        if (value < 0)
        {
            vitals.GetVital(type).DecreaseValue(-value);
        }
    }

    public override AInventoryItem? GetPerishedVersion() { return PerishedVersion; }
    public override AInventoryItem? GetCookedVersion() { return CookedVersion; }

    public override List<ItemInteractionType> GetInteractionOptions() => new List<ItemInteractionType>() { ItemInteractionType.Consume, ItemInteractionType.Discard };
}