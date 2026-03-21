public class Stim_Fruit : Stimulus
{
    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Hunger, -8);

        return interpretation;
    }

    public override void VisitAndInteract(MudyakBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(PlantOrCarcass))
            return;

        PlantOrCarcass carcass = AssociatedObject as PlantOrCarcass;

        brain.Eat(carcass);
    }

    public override string GetDescription()
    {
        return "a cactus";
    }
}
