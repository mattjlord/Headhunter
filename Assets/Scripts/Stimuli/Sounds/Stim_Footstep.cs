using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stim_Footstep : SoundStimulus
{
    [SerializeField] private OrganismType _organismType;

    public OrganismType OrganismType { set { _organismType = value; } }

    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);

        if (_organismType != OrganismType.Mudyak && Location.GetDistanceFrom(brain.Organism.Position, OrganismType.Hunter) < MudyakBrain.HunterThreatDistance)
        {
            interpretation.OverrideValence(-1);
            interpretation.OverridePriority(80);
        }

        return interpretation;
    }

    public virtual StimulusInterpretation VisitAndInterpret(BulletRaptorBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);

        if (_organismType != OrganismType.BulletRaptor)
            interpretation.AssignVitalImpact(VitalType.Hunger, -10);

        return interpretation;
    }

    public override string GetDescription()
    {
        return "footsteps from a " + _organismType;
    }
}
