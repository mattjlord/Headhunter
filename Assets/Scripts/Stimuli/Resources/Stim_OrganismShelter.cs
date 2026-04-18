using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stim_OrganismShelter : Stimulus
{
    [SerializeField] private OrganismType _organismType;

    protected override void Update()
    {
        base.Update();
        Fire();
    }

    public override StimulusInterpretation VisitAndInterpret(MudyakBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);

        if (_organismType == OrganismType.Mudyak)
            AssignImpact(interpretation, brain);

        return interpretation;
    }

    public override StimulusInterpretation VisitAndInterpret(BulletRaptorBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);

        if (_organismType == OrganismType.BulletRaptor)
            AssignImpact(interpretation, brain);

        return interpretation;
    }

    public override StimulusInterpretation VisitAndInterpret(ElectrowaspBrain brain)
    {
        StimulusInterpretation interpretation = GenerateBaseInterpretation(brain);

        if (_organismType == OrganismType.Electrowasp)
            AssignImpact(interpretation, brain);

        return interpretation;
    }



    private void AssignImpact(StimulusInterpretation interpretation, ABrain brain)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(OrganismShelter)) return;

        OrganismShelter shelter = AssociatedObject as OrganismShelter;

        if (!shelter.CanEnter(brain.Organism)) return;

        interpretation.AssignVitalImpact(VitalType.Exhaustion, -10);
        interpretation.AssignVitalImpact(VitalType.Injury, -10);
        interpretation.AssignVitalImpact(VitalType.Heat, -10);
    }

    public override void VisitAndInteract(MudyakBrain brain, StimulusResponseType type) => Interact(brain);
    public override void VisitAndInteract(BulletRaptorBrain brain, StimulusResponseType type) => Interact(brain);
    public override void VisitAndInteract(ElectrowaspBrain brain, StimulusResponseType type) => Interact(brain);

    private void Interact(ABrain brain)
    {
        if (AssociatedObject == null || AssociatedObject.GetType() != typeof(OrganismShelter)) return;

        OrganismShelter shelter = AssociatedObject as OrganismShelter;

        brain.Rest(shelter);
    }

    public override string GetDescription()
    {
        return "shelter for a " + _organismType;
    }
}
