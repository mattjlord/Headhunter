using System;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    [SerializeField] private float _memory;

    private OrganismType _organismType;

    private List<Stimulus> _activeStimuli;
    private Dictionary<Stimulus, float> _stimuliInMemory;

    public OrganismType OrganismType { set { _organismType = value; } }

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
            float elapsedTime = Time.fixedTime - entry.Value;
            if (elapsedTime > _memory)
            {
                toRemove.Add(entry.Key);
            }
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
        stimulus.OnDestroyed += RemoveStimulus;
    }

    private void RemoveStimulus(Stimulus stimulus)
    {
        _activeStimuli.Remove(stimulus);
        stimulus.DecrementObservers();
        stimulus.OnDestroyed -= RemoveStimulus;
    }

    public void ForgetStimulus(Stimulus stimulus)
    {
        _stimuliInMemory.Remove(stimulus);
        stimulus.DecrementObservers();
        stimulus.OnDestroyed -= ForgetStimulus;
    }

    public void StartForgettingStimulus(Stimulus stimulus)
    {
        _activeStimuli.Remove(stimulus);
        _stimuliInMemory.Add(stimulus, Time.fixedTime);
        stimulus.OnDestroyed += ForgetStimulus;
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

        float distanceToExistingStimulus = existingStimulus.Location.GetDistanceFrom(position, _organismType);
        float distanceToNewStimulus = newStimulus.Location.GetDistanceFrom(position, _organismType);

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
            Vector2? closestPoint = stimulus.Location.GetClosestPoint(VectorUtils.Vec3ToVec2(origin), _organismType);
            if (closestPoint == null) { continue; }
            Vector3 stimulusPosition = VectorUtils.Vec2ToVec3((Vector2)closestPoint) + Vector3.up * 3;
            Gizmos.DrawLine(origin, stimulusPosition);
        }

        if (_stimuliInMemory == null) return;

        Gizmos.color = Color.red;
        foreach (Stimulus stimulus in _stimuliInMemory.Keys)
        {
            Vector2? closestPoint = stimulus.Location.GetClosestPoint(VectorUtils.Vec3ToVec2(origin), _organismType);
            if (closestPoint == null) { continue; }
            Vector3 stimulusPosition = VectorUtils.Vec2ToVec3((Vector2)closestPoint) + Vector3.up * 3;
            Gizmos.DrawLine(origin, stimulusPosition);
        }
    }
}
