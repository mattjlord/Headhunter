public class Stim_BulletRaptor : Stimulus
{
    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Injury, 10);

        if (SenseType == SenseType.Sight)
        {
            interpretation.OverridePriority(90); // If it's in sight, flee
            interpretation.OverrideValence(1);
        }
        else
        {
            interpretation.OverridePriority(80); // Set it fairly high if its just smell
        }

        return interpretation;
    }

    public override void VisitAndInteract(MudyakBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(AIOrganism))
            return;

        Organism organism = AssociatedObject as Organism;

        brain.Attack(organism);
    }

    public override string GetDescription()
    {
        return "a bullet raptor";
    }
}