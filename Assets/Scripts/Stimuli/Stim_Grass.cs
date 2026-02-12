using UnityEngine;

public class Stim_Grass : Stimulus
{
    private void Update()
    {
        Fire();
    }

    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Hunger, -10);
        interpretation.AssignVitalImpact(VitalType.Thirst, -2);
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
