using UnityEngine;

public class Stim_Grass : AStimulus
{
    private void Update()
    {
        Fire();
    }

    public override StimulusInterpretation Visit(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        interpretation.AssignVitalImpact(VitalType.Hunger, -10);
        interpretation.AssignVitalImpact(VitalType.Thirst, -2);
        return interpretation;
    }
}
