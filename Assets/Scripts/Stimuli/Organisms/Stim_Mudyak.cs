using UnityEngine;

public class Stim_Mudyak : Stimulus
{
    public override StimulusInterpretation VisitAndInterpret(BulletRaptorBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Hunger, -10);
        interpretation.Hostile = true;

        if (SenseType == SenseType.Sight)
        {
            interpretation.OverridePriority(100); // If it can see the mudyak, set priority VERY HIGH
        }

        return interpretation;
    }

    public override void VisitAndInteract(BulletRaptorBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(AIOrganism))
            return;

        Organism organism = AssociatedObject as Organism;

        brain.Attack(organism);
    }

    public override StimulusInterpretation VisitAndInterpret(ElectrowaspBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);

        if (AssociatedObject != null && AssociatedObject.LeechOrganism != null && AssociatedObject.LeechOrganism != brain.Organism)
            return interpretation;

        if (ProducerOrganism != null && ProducerOrganism.LeechOrganism != null && ProducerOrganism.LeechOrganism != brain.Organism)
            return interpretation;

        //interpretation.AssignVitalImpact(VitalType.Hunger, -8);

        if (SenseType == SenseType.Sight)
            interpretation.AssignVitalImpact(VitalType.Hunger, -10);

        return interpretation;
    }

    public override void VisitAndInteract(ElectrowaspBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null)
            return;

        brain.Leech(AssociatedObject, OrganismType.Mudyak);
    }

    public override string GetDescription()
    {
        return "a mudyak";
    }
}