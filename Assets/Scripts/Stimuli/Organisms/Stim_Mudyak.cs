using UnityEngine;

public class Stim_Mudyak : Stimulus
{
    public override StimulusInterpretation VisitAndInterpret(ShellheadBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Hunger, -10);
        interpretation.CanEliminate = true;

        if (SenseType == SenseType.Sight)
        {
            interpretation.OverridePriority(90); // If it can see the mudyak, set priority VERY HIGH
        }

        return interpretation;
    }

    public override string GetDescription()
    {
        return "a mudyak";
    }
}