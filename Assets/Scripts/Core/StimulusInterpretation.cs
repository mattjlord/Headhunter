using System.Collections.Generic;
using UnityEngine;

public class StimulusInterpretation
{
    private AIOrganism _organism;

    private Dictionary<VitalType, float> _vitalImpactEstimate;

    private bool _hostile = false;

    private bool _overridePriority = false;
    private float _priorityOverrideValue = 0.0f;

    private bool _overrideValence = false;
    private float _valenceOverrideValue = 0.0f;

    public StimulusInterpretation(AIOrganism organism)
    {
        _organism = organism;
        _vitalImpactEstimate = new Dictionary<VitalType, float>
        {
            { VitalType.Hunger, 0f },
            { VitalType.Thirst, 0f },
            { VitalType.Exhaustion, 0f },
            { VitalType.Heat, 0f },
            { VitalType.Injury, 0f }
        };
    }

    public bool Hostile
    {
        get { return _hostile; }
        set { _hostile = value; }
    }

    public void OverridePriority(float value)
    {
        _priorityOverrideValue = value;
        _overridePriority = true;
    }

    public void OverrideValence(float value)
    {
        _valenceOverrideValue = value;
        _overrideValence = true;
    }

    public void AssignVitalImpact(VitalType vitalType, float impact)
    {
        _vitalImpactEstimate[vitalType] = impact;
    }

    public float EvaluatePriority()
    {
        if (_overridePriority)
        {
            return _priorityOverrideValue;
        }
        float sum = 0.0f;
        foreach (var entry in _vitalImpactEstimate)
        {
            VitalType vital = entry.Key;
            float rawVitalVal = _organism.Vitals.GetVital(vital).Value;
            float biasedVitalVal = _organism.HerdManagement.GetHerdBiasedVitalValue(rawVitalVal, vital);
            float vitalImpact = _vitalImpactEstimate[vital];
            sum += biasedVitalVal * Mathf.Abs(vitalImpact);
        }
        return Mathf.Clamp(sum * 0.11f, 0, 100);
    }

    private float EvaluateValence()
    {
        if (_overrideValence)
        {
            return _valenceOverrideValue;
        }
        float sum = 0.0f;
        foreach (var entry in _vitalImpactEstimate)
        {
            VitalType vital = entry.Key;
            float rawVitalVal = _organism.Vitals.GetVital(vital).Value;
            float biasedVitalVal = _organism.HerdManagement.GetHerdBiasedVitalValue(rawVitalVal, vital);
            float vitalImpact = _vitalImpactEstimate[vital];
            sum += biasedVitalVal * vitalImpact;
        }
        return sum;
    }

    public StimulusResponseType EvaluateResponseType()
    {
        float valence = EvaluateValence();
        if (valence == 0)
        {
            return StimulusResponseType.Ignore;
        } else if (valence < 0)
        {
            return StimulusResponseType.Pursue;
        }
        if (_hostile)
        {
            return StimulusResponseType.Eliminate;
        }
        else
        {
            return StimulusResponseType.Flee;
        }
    }
}
