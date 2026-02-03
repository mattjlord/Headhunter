using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HerdManagement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _herdBehavior;
    [SerializeField] List<Organism> _herd;

    private float GetHerdVitalAverage(VitalType vital)
    {
        List<float> values = new List<float>();
        foreach (Organism organism in _herd)
        {
            float vitalValue = organism.Vitals.GetVital(vital).Value;
            values.Add(vitalValue);
        }
        return values.Average();
    }

    public float GetHerdBiasedVitalValue(float rawValue, VitalType vital)
    {
        if (_herd.Count == 0)
        {
            return rawValue;
        }
        float herdValue = GetHerdVitalAverage(vital);
        float lerp = _herdBehavior / 100;
        float biasedValue = Mathf.Lerp(rawValue, herdValue, lerp);
        return biasedValue;
    }
}
