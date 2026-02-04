using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudyakBrain : ABrain
{
    public override StimulusInterpretation AcceptStimulus(AStimulus stimulus)
    {
        return stimulus.Visit(this);
    }
}
