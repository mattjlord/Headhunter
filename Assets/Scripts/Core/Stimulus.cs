using System.Collections.Generic;
using UnityEngine;

public class Stimulus : MonoBehaviour
{
    [SerializeField] private ALocation _location;
    [SerializeField] private SenseType _senseType;
    [SerializeField] private float _detectableDistance;
    [SerializeField] private bool _lingering = false;

    [SerializeField] private WorldObject? _associatedObject;

    private int _observers = 0;

    private Organism? _producerOrganism;

    private void Update()
    {
        if (_observers == 0 && !_lingering)
        {
            Destroy(gameObject);
        }
    }

    public ALocation Location { 
        get { return _location; }
        set { _location = value; }
    }
    public SenseType SenseType { 
        get { return _senseType; } 
        set { _senseType = value; }
    }
    public WorldObject? AssociatedObject { 
        get { return _associatedObject; }
        set { _associatedObject = value; }
    }
    public Organism? ProducerOrganism
    {
        get { return _producerOrganism; }
        set { _producerOrganism = value; }
    }
    public float DetectableDistance { 
        get { return _detectableDistance; } 
        set { _detectableDistance = value; }
    }
    public bool Lingering
    {
        get { return _lingering; }
        set { _lingering = value; }
    }
    public bool IsInteractible { get { return _associatedObject != null; } }

    public bool WithinReach(Organism organism)
    {
        if (_associatedObject == null)
            return false;

        float reach = organism.Reach;

        AreaLocation areaLocation = _associatedObject.AreaLocation;
        float dist;
        if (areaLocation != null)
        {
            if (areaLocation.LocationReached(organism.Position))
                return true;
            dist = areaLocation.GetDistanceFrom(organism.Position);
        }
        else
        {
            dist = Vector2.Distance(organism.Position, _associatedObject.Position);
        }

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
                found.Add(brain);
            }
        }

        return found;
    }

    public void IncrementObservers()
    {
        _observers++;
    }

    public void DecrementObservers()
    {
        _observers--;
    }

    // Visitors
    public virtual StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        return GenerateBaseInterpretation(brain);
    }
    public virtual void VisitAndInteract(MudyakBrain brain, StimulusResponseType type) { }

    public virtual StimulusInterpretation VisitAndInterpret(ShellheadBrain brain)
    {
        return GenerateBaseInterpretation(brain);
    }
    public virtual void VisitAndInteract(ShellheadBrain brain, StimulusResponseType type) { }

    public virtual string GetDescription()
    {
        return "a stimulus";
    }
}