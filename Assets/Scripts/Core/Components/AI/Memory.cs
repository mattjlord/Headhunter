using System;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    [SerializeField] private float _memory;

    private List<Stimulus> _activeStimuli;
    private Dictionary<Stimulus, float> _stimuliInMemory;

    private void Awake()
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

        Stimulus existingStimulus = null;

        foreach (Stimulus activeStimulus in _activeStimuli)
        {
            if (activeStimulus.GetType() == stimulus.GetType())
            {
                existingStimulus = activeStimulus;
                break;
            }
        }

        if (existingStimulus != null)
            ForgetStimulus(existingStimulus);
        
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
        Debug.Log("Started forgetting");
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

    public bool HasCloserSimilarStimulus(Stimulus newStimulus, Vector2 position)
    {
        Type stimType = newStimulus.GetType();

        Stimulus existingStimulus = null;

        foreach (Stimulus activeStimulus in _activeStimuli)
        {
            if (activeStimulus.GetType() == stimType)
            {
                existingStimulus = activeStimulus;
                break;
            }
        }

        if (existingStimulus == null)
            return false;

        float distanceToExistingStimulus = existingStimulus.Location.GetDistanceFrom(position);
        float distanceToNewStimulus = newStimulus.Location.GetDistanceFrom(position);

        return distanceToNewStimulus >= distanceToExistingStimulus;
    }

    private void OnDrawGizmos()
    {
        if (_activeStimuli == null)
            return;

        Gizmos.color = Color.yellow;
        Vector3 origin = transform.position + Vector3.up * 3;
        foreach (Stimulus stimulus in _activeStimuli)
        {
            Vector3 stimulusPosition = VectorUtils.Vec2ToVec3(stimulus.Location.GetClosestPoint(VectorUtils.Vec3ToVec2(origin))) + Vector3.up * 3;
            Gizmos.DrawLine(origin, stimulusPosition);
        }
    }
}
