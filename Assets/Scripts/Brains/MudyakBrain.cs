using UnityEngine;

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

    public override void Attack(Organism obj)
    {
        // TODO: Make this more unique, this is just the bullet raptor code currently
        if (!obj || !organism.ActionManagement.IsReadyForQueue())
            return;

        OrganismAction action = new OrganismAction(organism);

        if (obj.WithinReach(organism.Position, organism.Reach))
        {
            Debug.Log("Attacking");
            action.Duration = 0.3f; // Placeholder
            action.TriggerDelay = 0.05f; // Placeholder
            action.TriggeredAction = () =>
            {
                if (obj)
                {
                    organism.MeleeAttack.AttackOrganism(obj);
                }
            };
        }
        else
        {
            Debug.Log("Charging!");
            Vector2 target = obj.Position + 0.25f * obj.Movement.Velocity;

            Vector2 targetToThis = (organism.Position - target).normalized;
            target += targetToThis * (obj.Radius + organism.Reach);

            action.Duration = 0.2f; // Placeholder
            action.Displacement = target - organism.Position;
        }

        organism.ActionManagement.QueueAction(action);
    }
}
