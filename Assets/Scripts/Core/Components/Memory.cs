using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    private List<Stimulus> _activeStimuli;
    private List<Stimulus> _blockedStimuli;

    private void Start()
    {
        _activeStimuli = new List<Stimulus>();
        _blockedStimuli = new List<Stimulus>();
    }

    public void AddStimulus(Stimulus stimulus)
    {
        _activeStimuli.Add(stimulus);
        stimulus.IncrementObservers();
    }

    public void ForgetStimulus(Stimulus stimulus)
    {
        // TODO: Implement actual memory & forgetting after a period of time
        _activeStimuli.Remove(stimulus);
        stimulus.DecrementObservers();
    }

    public bool IsStimulusActive(Stimulus stimulus)
    {
        return _activeStimuli.Contains(stimulus);
    }
}
