public class Stim_Glowvine : Stimulus
{
    protected virtual void Update()
    {
        base.Update();
        Fire();
    }

    public override StimulusInterpretation VisitAndInterpret(ElectrowaspBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Thirst, -10);
        return interpretation;
    }

    public override void VisitAndInteract(ElectrowaspBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(PlantOrCarcass))
            return;

        FoodOrWaterObject obj = AssociatedObject as FoodOrWaterObject;

        brain.Drink(obj);
    }

    public override string GetDescription()
    {
        return "water";
    }
}
