using UnityEngine;

public class StimulusResponseTask : BehaviorTask
{
    private Stimulus _stimulus;
    private StimulusResponseType _responseType;
    private ABrain _brain;

    public StimulusResponseTask(AIOrganism organism, Stimulus stimulus, StimulusResponseType responseType, ABrain brain) : base(organism)
    {
        _stimulus = stimulus;
        _responseType = responseType;
        _brain = brain;
    }

    public override void Update()
    {
        if (!Organism.Senses.CanSense(_stimulus))
        {
            Priority = 0;
        }

        if (Priority == 0)
        {
            Organism.Memory.ForgetStimulus(_stimulus);
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
            stimulusReached = _stimulus.WithinReach(Organism);
        }
        else
        {
            stimulusReached = stimulusLocation.LocationReachedByOrganism(Organism);
        }

        if (!stimulusReached)
        {
            Organism.Navigation.MoveTowards(Organism, stimulusLocation.GetClosestPoint(Organism.Position));
            description = "Pursuing stimulus";
            return;
        }

        // Stimulus has been reached, stop movement
        Organism.Navigation.StopMovement(Organism);

        if (!_stimulus.IsInteractible) // Nothing to do but wander
        {
            Organism.Navigation.WanderAround(Organism, stimulusLocation);
            description = "Location reached, wandering around";
            return;
        }

        _brain.AcceptAndInteract(_stimulus, _responseType);
    }

    private void FleeStimulus()
    {
        ALocation stimulusLocation = _stimulus.Location;
        Organism.Navigation.MoveAwayFrom(Organism, stimulusLocation.GetClosestPoint(Organism.Position));
        description = "Fleeing stimulus";
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
