using UnityEngine;

public class StimulusResponseTask : BehaviorTask
{
    private Stimulus _stimulus;
    private StimulusResponseType _responseType;
    private ABrain _brain;
    private bool _hostile;

    private float _threatCheckInterval = 5f;
    private float _lastThreatCheck;

    public StimulusResponseTask(AIOrganism organism, Stimulus stimulus, StimulusResponseType responseType, ABrain brain, bool hostile) : base(organism)
    {
        _stimulus = stimulus;
        _responseType = responseType;
        _brain = brain;
        _hostile = hostile;
    }

    public override void Update()
    {
        if (!_stimulus)
        {
            Priority = 0;
            return;
        }
        if (Organism.Memory.IsStimulusActive(_stimulus) && !Organism.Senses.CanSense(_stimulus))
        {
            if (_responseType == StimulusResponseType.Flee)
                _lastThreatCheck = Time.time;
            Organism.Memory.StartForgettingStimulus(_stimulus);
        }

        if (!Organism.Memory.IsStimulusActive(_stimulus) && !Organism.Memory.CanRemember(_stimulus))
        {
            Priority = 0;
            return;
        }

        switch (_responseType)
        {
            case StimulusResponseType.Pursue:
                PursueStimulus();
                return;
            case StimulusResponseType.Eliminate:
                PursueStimulus();
                return;
            case StimulusResponseType.Flee:
                FleeStimulus();
                return;
        }
    }

    private void PursueStimulus()
    {
        ALocation stimulusLocation = _stimulus.Location;
        bool stimulusReached;
        if (_stimulus.IsInteractible)
        {
            stimulusReached = _stimulus.WithinReach(Organism, _hostile);
        }
        else
        {
            stimulusReached = stimulusLocation.LocationReachedByOrganism(Organism);
        }

        if (!stimulusReached)
        {
            Vector2? closestPoint = stimulusLocation.GetClosestPoint(Organism.Position, Organism.OrganismType);
            if (closestPoint == null) // Not reachable anymore, forget this
            {
                Priority = 0;
                return;
            }
            Organism.Navigation.MoveTowards(Organism, (Vector2)closestPoint, _hostile, !_stimulus.Fixed);
            description = "Pursuing stimulus";
            return;
        }

        // Stimulus has been reached, stop movement
        Organism.Navigation.StopMovement(Organism);

        if (!_stimulus.IsInteractible) // Nothing to do but wander
        {
            Organism.Navigation.WanderAround(Organism, stimulusLocation, false);
            description = "Location reached, wandering around";
            return;
        }

        _brain.AcceptAndInteract(_stimulus, _responseType);
    }

    private void FleeStimulus()
    {
        ALocation stimulusLocation = _stimulus.Location;

        WorldObject stimObj = _stimulus.AssociatedObject;
        Organism pursuer = null;
        if (stimObj != null && (stimObj.GetType() == typeof(AIOrganism) || stimObj.GetType() == typeof(PlayerOrganism)))
            pursuer = stimObj as Organism;

        if (pursuer != null) // Fleeing an organism, we have additional logic here
        {
            if (pursuer.WithinReach(Organism.Position, Organism.CombatReach)) // If the organism is within combat range, decide whether to fight or keep running
            {
                OnCornered();
            }
        }

        Vector2? closestPoint = stimulusLocation.GetClosestPoint(Organism.Position, Organism.OrganismType);

        if (closestPoint == null)
        {
            Priority = 0;
            return;
        }

        if (Organism.Memory.CanRemember(_stimulus) && Time.time > _lastThreatCheck + _threatCheckInterval)
        {
            _lastThreatCheck = Time.time;
            bool threatened = CheckForThreat();

            if (!threatened)
            {
                Organism.Memory.ForgetStimulus(_stimulus);
                return;
            }
        }

        bool canFlee = Organism.Navigation.MoveAwayFrom(Organism, (Vector2)closestPoint, true);
        if (canFlee)
            description = "Fleeing stimulus";
        else // Organism is cornered
        {
            if (pursuer != null)
            {
                OnCornered();
            }
            // Unhandled behavior: cornered by a non-organism (this should never happen)
        }
    }

    private bool CheckForThreat()
    {
        if (_stimulus.SenseType == SenseType.Sight)
        {
            Vector2? point = _stimulus.Location.GetClosestPoint(Organism.Position, Organism.OrganismType);
            if (point == null) { return false; }
            Vector2 lookDir = ((Vector2)point - Organism.Position).normalized;
            Organism.LookDirection = lookDir;
        }

        return Organism.Senses.CanSense(_stimulus);
    }

    private void OnCornered()
    {
        Debug.Log("Time to fight!");
        _responseType = StimulusResponseType.Eliminate;
        _hostile = true;
        PursueStimulus();
        return;
    }

    public override string GetName()
    {
        switch (_responseType)
        {
            case StimulusResponseType.Pursue:
                return "Responding to stimulus (goal: pursue)";
            case StimulusResponseType.Eliminate:
                return "Responding to stimulus (goal: eliminate)";
            case StimulusResponseType.Flee:
                return "Responding to stimulus (goal: flee)";
            default:
                return "Ignoring stimulus";
        }
    }

    public override bool HasAssociatedLocation(ALocation location)
    {
        return _stimulus.Location.Equals(location);
    }
}
