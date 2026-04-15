using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    [SerializeField] private float _visibleDistance;
    [SerializeField] private Stimulus _visibilityStimulus;

    [SerializeField] private Stimulus _overrideStimulus;

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
            _visibilityStimulus.Fixed = false;
        }
    }

    private void Update()
    {
        if (_overrideStimulus == null)
            _visibilityStimulus.Fire();
        else
            _overrideStimulus.Fire();
    }

    public float VisibleDistance {
        get { return _visibleDistance; }
        set 
        { 
            _visibleDistance = value;
            _visibilityStimulus.DetectableDistance = value;
        }
    }

    public Stimulus OverrideStimulus
    {
        get { return _overrideStimulus;}
        set 
        { 
            _overrideStimulus = value;
            if (value != null)
            {
                _overrideStimulus.DetectableDistance = _visibleDistance;
                _overrideStimulus.SenseType = SenseType.Sight;
                _overrideStimulus.AssociatedObject = _organism;
                _overrideStimulus.Lingering = true;
                _overrideStimulus.Fixed = false;
            }
        }
    }
}
