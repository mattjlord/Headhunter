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
            interpretation.OverridePriority(90); // If it can see the mudyak, set priority VERY HIGH
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

    public override string GetDescription()
    {
        return "a mudyak";
    }
}