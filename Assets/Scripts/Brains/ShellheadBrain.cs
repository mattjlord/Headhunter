public class ShellheadBrain : ABrain
{
    public override void AcceptAndInteract(Stimulus stimulus, StimulusResponseType type)
    {
        stimulus.VisitAndInteract(this, type);
    }

    public override StimulusInterpretation AcceptAndInterpret(Stimulus stimulus)
    {
        return stimulus.VisitAndInterpret(this);
    }
}
