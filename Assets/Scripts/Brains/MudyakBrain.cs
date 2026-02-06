public class MudyakBrain : ABrain
{
    public override StimulusInterpretation AcceptAndInterpret(Stimulus stimulus)
    {
        return stimulus.VisitAndInterpret(this);
    }

    public override void AcceptAndInteract(Stimulus stimulus, StimulusResponseType type)
    {
        stimulus.VisitAndInteract(this, type);
    }
}
