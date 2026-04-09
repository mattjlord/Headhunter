using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stim_Gunshot : SoundStimulus
{
    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Injury, 10);
        return interpretation;
    }

    public virtual StimulusInterpretation VisitAndInterpret(BulletRaptorBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Injury, 10);
        interpretation.AssignVitalImpact(VitalType.Hunger, -10);
        return interpretation;
    }
}
