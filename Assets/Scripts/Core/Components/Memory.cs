using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    [SerializeField] private float _memory;

    private List<Stimulus> _activeStimuli;
    private Dictionary<Stimulus, float> _stimuliInMemory;

    private void Start()
    {
        _activeStimuli = new List<Stimulus>();
        _stimuliInMemory = new Dictionary<Stimulus, float>();
    }

    private void FixedUpdate()
    {
        List<Stimulus> toRemove = new List<Stimulus>();
        foreach (var entry in _stimuliInMemory)
        {
            if (Time.fixedTime > entry.Value + _memory)
                toRemove.Add(entry.Key);
        }

        foreach (Stimulus stimulus in toRemove)
        {
            ForgetStimulus(stimulus);
        }
    }

    public void AddStimulus(Stimulus stimulus)
    {
        if (_stimuliInMemory.ContainsKey(stimulus))
        {
            _stimuliInMemory.Remove(stimulus);
        }
        
        _activeStimuli.Add(stimulus);
        stimulus.IncrementObservers();
    }

    private void ForgetStimulus(Stimulus stimulus)
    {
        Debug.Log("Forgetting " + stimulus.GetType().Name);
        _stimuliInMemory.Remove(stimulus);
        stimulus.DecrementObservers();
    }

    public void StartForgettingStimulus(Stimulus stimulus)
    {
        _activeStimuli.Remove(stimulus);
        _stimuliInMemory.Add(stimulus, Time.fixedTime);
    }

    public bool IsStimulusActive(Stimulus stimulus)
    {
        return _activeStimuli.Contains(stimulus);
    }

    public bool CanRemember(Stimulus stimulus)
    {
        return _stimuliInMemory.ContainsKey(stimulus);
    }
}
