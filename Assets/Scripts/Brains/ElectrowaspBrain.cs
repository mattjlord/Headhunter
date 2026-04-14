using UnityEngine;

public class ElectrowaspBrain : ABrain
{
    private static float FeedRate = -5f;

    private bool _leeching = false;
    public bool Leeching { get => _leeching; set => _leeching = value; }

    public override void AcceptAndInteract(Stimulus stimulus, StimulusResponseType type)
    {
        stimulus.VisitAndInteract(this, type);
    }

    public override StimulusInterpretation AcceptAndInterpret(Stimulus stimulus)
    {
        return stimulus.VisitAndInterpret(this);
    }

    public override void Attack(Organism obj)
    {
        // TODO
    }

    public void Leech(WorldObject obj, OrganismType organismType)
    {
        if (organism.Vitals.GetVital(VitalType.Hunger).Value < 0.1f)
        {
            obj.LeechOrganism = null;
            _leeching = false;
            return;
        }

        _leeching = true;
        obj.LeechOrganism = organism;

        if (organismType != OrganismType.Mudyak) return;

        organism.Movement.HideFromNavMesh();
        transform.position = VectorUtils.Vec2ToVec3(obj.Position) + Vector3.up;

        if (obj is Organism)
        {
            Organism org = (Organism)obj;
            organism.LookDirection = org.LookDirection;
        }

        ActionManagement actionManagement = organism.ActionManagement;

        if (actionManagement.IsReadyForQueue())
        {
            OrganismAction action = new OrganismAction(organism);
            action.AnimationName = "Eat";
            action.Duration = 2.5f;
            action.TriggerDelay = 1f;
            action.TriggeredAction = () => organism.Vitals.GetVital(VitalType.Hunger).Value += FeedRate;
            actionManagement.QueueAction(action);
        }
        
    }
}
