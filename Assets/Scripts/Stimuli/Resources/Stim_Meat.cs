using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stim_Meat : Stimulus
{
    protected override void Update()
    {
        base.Update();
        Fire();
    }

    public override StimulusInterpretation VisitAndInterpret(BulletRaptorBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Hunger, -10);
        interpretation.Hostile = true;

        return interpretation;
    }

    public override void VisitAndInteract(BulletRaptorBrain brain, StimulusResponseType type)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(PlantOrCarcass))
            return;

        PlantOrCarcass carcass = AssociatedObject as PlantOrCarcass;

        brain.Eat(carcass);
    }

    public override string GetDescription()
    {
        return "meat";
    }
}
