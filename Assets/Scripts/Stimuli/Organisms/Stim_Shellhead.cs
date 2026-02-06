public class Stim_Shellhead : Stimulus
{
    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);
        //interpretation.AssignVitalImpact(VitalType.Injury, 10);

        if (SenseType == SenseType.Sight)
        {
           // interpretation.OverridePriority(90); // If it's in sight, flee
           // interpretation.OverrideValence(1);
        }

        return interpretation;
    }

    public override string GetDescription()
    {
        return "a shellhead";
    }
}