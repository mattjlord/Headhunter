using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStimulus : MonoBehaviour
{
    [SerializeField] private ALocation _location;
    [SerializeField] private SenseType _senseType;
    [SerializeField] private float _detectableDistance;

    [SerializeField] private bool _isInteractible;

    [SerializeField] private WorldObject? _associatedObject;

    public ALocation Location { get { return _location; } }
    public SenseType SenseType { get { return _senseType; } }
    public WorldObject? AssociatedObject { get { return _associatedObject; } }
    public float DetectableDistance { get { return _detectableDistance; } }
    public bool IsInteractible { get { return _isInteractible; } }

    public abstract StimulusInterpretation Visit(MudyakBrain brain);

    public bool WithinReach(Organism organism)
    {
        if (_location.LocationReached(organism.Position))
            return true;
        float reach = organism.Reach;
        float dist = _location.GetDistanceFrom(organism.Position);
        return dist <= reach;
    }

    protected StimulusInterpretation GenerateBaseInterpretation(ABrain brain)
    {
        return new StimulusInterpretation(brain.Organism);
    }

    public void Fire()
    {
        List<ABrain> foundBrains = FindBrainsInRange();
        foreach (ABrain brain in foundBrains)
        {
            brain.RespondToStimulus(this);
        }
    }

    private List<ABrain> FindBrainsInRange()
    {
        List<ABrain> found = new List<ABrain>();
        List<Collider> colliders = _location.GetNearbyColliders(_detectableDistance);
        foreach (Collider collider in colliders)
        {
            ABrain brain = collider.GetComponent<ABrain>();
            if (brain != null)
            {
                Debug.DrawLine(transform.position, collider.transform.position);
                found.Add(brain);
            }
        }

        return found;
    }
}