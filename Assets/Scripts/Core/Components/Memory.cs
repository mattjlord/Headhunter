using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    private List<AStimulus> _activeStimuli;
    private List<AStimulus> _blockedStimuli;

    private void Start()
    {
        _activeStimuli = new List<AStimulus>();
        _blockedStimuli = new List<AStimulus>();
    }

    public void AddStimulus(AStimulus stimulus)
    {
        _activeStimuli.Add(stimulus);
    }

    public void ForgetStimulus(AStimulus stimulus)
    {
        // TODO: Implement actual memory & forgetting after a period of time
        _activeStimuli.Remove(stimulus);
    }

    public bool IsStimulusActive(AStimulus stimulus)
    {
        return _activeStimuli.Contains(stimulus);
    }
}
