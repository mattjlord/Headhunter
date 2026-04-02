using UnityEngine;

public class Stim_Grass : Stimulus
{
    protected override void Update()
    {
        base.Update();
        Fire();
    }

    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Hunger, -3);
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
