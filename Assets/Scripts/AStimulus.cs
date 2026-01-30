using System;
using UnityEngine;

public abstract class AStimulus : MonoBehaviour
{
    [SerializeField] private ALocation _location;
    [SerializeField] private SenseType _senseType;

    [SerializeField] private WorldObject? _associatedObject;

    public ALocation Location { get { return _location; } }
    public SenseType SenseType { get { return _senseType; } }
    public WorldObject? AssociatedObject { get { return _associatedObject; } }

    public abstract StimulusInterpretation Visit(MudyakBrain brain);
    
    public bool WithinReach(Organism organism)
    {
        if (_location.LocationReached(organism.Position))
            return true;
        float reach = organism.Reach;
        float dist = _location.GetDistanceFrom(organism.Position);
        return dist <= reach;
    }
}
