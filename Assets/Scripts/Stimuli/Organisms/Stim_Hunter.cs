using UnityEngine;

public class Stim_Hunter : Stimulus
{
    public override StimulusInterpretation VisitAndInterpret(BulletRaptorBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Hunger, -10);
        interpretation.Hostile = true;

        if (SenseType == SenseType.Sight)
        {
            interpretation.OverridePriority(100); // If it can see the hunter, set priority VERY HIGH
        }
        else
        {
            interpretation.OverridePriority(80); // Set it fairly high if its just smell
        }

        return interpretation;
    }

    public override void VisitAndInteract(BulletRaptorBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(PlayerOrganism))
            return;

        Organism organism = AssociatedObject as Organism;

        brain.Attack(organism);
    }

    public override void VisitAndInteract(MudyakBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(PlayerOrganism))
            return;

        Organism organism = AssociatedObject as Organism;

        brain.Attack(organism);
    }

    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        float dist = Location.GetDistanceFrom(brain.Organism.Position, OrganismType.Hunter);
        if (dist < MudyakBrain.HunterThreatDistance)
        {
            interpretation.AssignVitalImpact(VitalType.Injury, 10);

            if (SenseType == SenseType.Sight)
            {
                interpretation.OverridePriority(100); // If the hunter is in sight, flee
                interpretation.OverrideValence(1);
            }
        }
        return interpretation;
    }

    public override string GetDescription()
    {
        return "the hunter";
    }
}