public abstract class StimulusResponseTask : BehaviorTask
{
    private AStimulus _stimulus;
    private StimulusResponseType _responseType;

    public StimulusResponseTask(AIOrganism organism, AStimulus stimulus, StimulusResponseType responseType) : base(organism)
    {
        _stimulus = stimulus;
        _responseType = responseType;
    }

    public override void Update()
    {
        if (!Organism.Senses.CanSense(_stimulus))
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
                EliminateStimulus();
                return;
            case StimulusResponseType.Flee:
                FleeStimulus();
                return;
        }
    }

    private void PursueStimulus()
    {
        ALocation stimulusLocation = _stimulus.Location;
        if (!_stimulus.WithinReach(Organism))
            Organism.Navigation.MoveTowards(Organism, stimulusLocation.GetClosestPoint(Organism.Position));
        else { }
            // TODO: Interact with stimulus
    }

    private void EliminateStimulus()
    {
        ALocation stimulusLocation = _stimulus.Location;
        if (!_stimulus.WithinReach(Organism))
            Organism.Navigation.MoveTowards(Organism, stimulusLocation.GetClosestPoint(Organism.Position));
        else { }
        // TODO: Interact with stimulus
    }

    private void FleeStimulus()
    {
        ALocation stimulusLocation = _stimulus.Location;
        Organism.Navigation.MoveAwayFrom(Organism, stimulusLocation.GetClosestPoint(Organism.Position));
    }
}
