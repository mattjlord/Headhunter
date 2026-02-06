using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    [SerializeField] private float _visibleDistance;
    [SerializeField] private Stimulus _visibilityStimulus;

    private Organism _organism;

    public Organism Organism
    {
        get { return _organism; }
        set
        {
            _organism = value;
            _visibilityStimulus.DetectableDistance = _visibleDistance;
            _visibilityStimulus.SenseType = SenseType.Sight;
            _visibilityStimulus.AssociatedObject = _organism;
            _visibilityStimulus.Lingering = true;
        }
    }

    private void FixedUpdate()
    {
        _visibilityStimulus.Fire();
    }

    public float VisibleDistance {
        get { return _visibleDistance; }
        set 
        { 
            _visibleDistance = value;
            _visibilityStimulus.DetectableDistance = value;
        }
    }
}
