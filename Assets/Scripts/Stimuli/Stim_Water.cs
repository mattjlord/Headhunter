public class Stim_Water : Stimulus
{
    private void Update()
    {
        Fire();
    }

    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Thirst, -10);
        return interpretation;
    }

    public override StimulusInterpretation VisitAndInterpret(BulletRaptorBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Thirst, -10);
        return interpretation;
    }

    public override void VisitAndInteract(MudyakBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(FoodOrWaterObject))
            return;

        FoodOrWaterObject obj = AssociatedObject as FoodOrWaterObject;

        brain.Eat(obj);
    }

    public override string GetDescription()
    {
        return "grass";
    }
}
