using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class Stimulus : MonoBehaviour
{
    public event Action<Stimulus> OnDestroyed;

    [SerializeField] private ALocation _location;
    [SerializeField] private SenseType _senseType;
    [SerializeField] protected float detectableDistance;
    [SerializeField] private bool _lingering = false;
    [SerializeField] private bool _fixed = true;

    [SerializeField] private WorldObject? _associatedObject;

    [SerializeField] private int _observers = 0;

    private float _spawnTime;
    private float _minLifetime = 1f;

    private Organism? _producerOrganism;
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
        get { return detectableDistance; } 
        set { detectableDistance = value; }
    }
    public bool Lingering
    {
        get { return _lingering; }
        set { _lingering = value; }
    }

    public bool Fixed
    {
        get { return _fixed; }
        set { _fixed = value; }
    }

    public bool IsInteractible { get { return _associatedObject != null; } }

    private void Start()
    {
        _spawnTime = Time.time;
    }

    protected virtual void Update()
    {
        if (_observers == 0 && !_lingering && Time.time > _spawnTime + _minLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public bool WithinReach(Organism organism, bool useCombatReach)
    {
        if (_associatedObject == null)
            return false;

        float reach;
        if (useCombatReach)
            reach = organism.CombatReach;
        else
            reach = organism.Reach;

        AreaLocation areaLocation = _associatedObject.AreaLocation;
        float dist;
        if (areaLocation != null)
        {
            if (areaLocation.LocationReached(organism.Position))
                return true;
            dist = areaLocation.GetDistanceFrom(organism.Position, organism.OrganismType);
        }
        else
        {
            dist = Vector2.Distance(organism.Position, _associatedObject.Position) - _associatedObject.Radius;
        }

        return dist <= reach;
    }

    protected StimulusInterpretation GenerateBaseInterpretation(ABrain brain)
    {
        return new StimulusInterpretation(brain.Organism);
    }

    public void Fire()
    {
        if (_location.GetType() == typeof(PointLocation))
        {
            //DrawUtils.DrawCircle(transform.position + Vector3.up, detectableDistance, Color.magenta, 1f);
        }
        foreach (AIOrganism organism in MasterOrganismManager.AllOrganisms)
        {
            organism.RespondToStimulus(this);
        }

        OnFire();
    }

    protected virtual void OnFire() { }

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

    public virtual StimulusInterpretation VisitAndInterpret(BulletRaptorBrain brain)
    {
        return GenerateBaseInterpretation(brain);
    }
    public virtual void VisitAndInteract(BulletRaptorBrain brain, StimulusResponseType type) { }

    public virtual StimulusInterpretation VisitAndInterpret(ElectrowaspBrain brain)
    {
        return GenerateBaseInterpretation(brain);
    }
    public virtual void VisitAndInteract(ElectrowaspBrain brain, StimulusResponseType type) { }

    public virtual string GetDescription()
    {
        return "a stimulus";
    }
}