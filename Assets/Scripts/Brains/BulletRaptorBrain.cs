using UnityEngine;

public class BulletRaptorBrain : ABrain
{

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
        if (!obj || !organism.ActionManagement.IsReadyForQueue())
            return;

        OrganismAction action = new OrganismAction(organism);

        if (obj.WithinReach(organism.Position, organism.Reach))
        {
            action.AnimationName = "Attack";
            action.TriggerDelay = 0.3f;
            action.Duration = 1.25f;
            action.TriggeredAction = () =>
            {
                if (obj)
                    organism.MeleeAttack.AttackOrganism(obj);
            };
        }
        else
        {
            Vector2 target = obj.Position + 0.25f * obj.Movement.Velocity;

            Vector2 targetToThis = (organism.Position - target).normalized;
            target += targetToThis * (obj.Radius + organism.Reach);

            action.AnimationName = "Charge";
            action.Duration = 0.625f;
            action.Displacement = target - organism.Position;
            action.DisplacementDelay = 0.4f;
        }

        organism.ActionManagement.QueueAction(action);
    }
}
