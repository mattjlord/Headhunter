using UnityEngine;

public class MudyakBrain : ABrain
{
    public static float HunterThreatDistance = 70f;

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
        // TODO: Make this more unique, this is just the bullet raptor code currently
        if (!obj || !organism.ActionManagement.IsReadyForQueue())
            return;

        OrganismAction action = new OrganismAction(organism);

        if (obj.WithinReach(organism.Position, organism.Reach))
        {
            action.AnimationName = "Attack";
            action.Duration = 1.666667f;
            action.TriggerDelay = 0.2f;
            action.TriggeredAction = () =>
            {
                if (obj)
                {
                    organism.MeleeAttack.AttackOrganism(obj, organism);
                }
            };
        }
        else
        {
            Vector2 target = obj.Position + 0.25f * obj.Movement.Velocity;

            Vector2 targetToThis = (organism.Position - target).normalized;
            target += targetToThis * (obj.Radius + organism.Reach);

            action.AnimationName = "Charge";
            action.Duration = 0.83333f;
            action.Displacement = target - organism.Position;
            action.DisplacementDelay = 0.2f;
        }

        organism.ActionManagement.QueueAction(action);
    }
}
