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
        if (!organism.ActionManagement.IsReadyForQueue())
            return;

        OrganismAction action = new OrganismAction(organism);

        if (obj.WithinReach(organism.Position, organism.Reach))
        {
            Debug.Log("Within biting range!");
            action.Duration = 0.2f; // Placeholder
            // TODO: Attack the target
        }
        else
        {
            Vector2 target = obj.Position + (2 * obj.Movement.Velocity);

            Vector2 targetToThis = (organism.Position - target).normalized;
            target += targetToThis * (obj.Radius + organism.Reach);

            action.Duration = 0.1f; // Placeholder
            action.Displacement = target - organism.Position;
        }

        organism.ActionManagement.QueueAction(action);
    }
}
