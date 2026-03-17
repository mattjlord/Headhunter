using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [SerializeField] private float _patience;

    private bool _wandering = false;

    public void MoveTowards(Organism organism, Vector2 pos, bool run, bool chasing = false)
    {
        organism.Movement.Move(organism, pos, run);
    }

    public bool MoveAwayFrom(Organism organism, Vector2 pos, bool run)
    {
        Vector2 lookDir = (organism.Position - pos).normalized;
        organism.LookDirection = lookDir;

        Vector2? pointAhead = organism.Senses.GetPointAhead(organism.Radius, organism.OrganismType);

        if (pointAhead == null)
        {
            // TODO: Somehow integrate behavior for when the organism gets cornered
            StopMovement(organism);
            return false;
        }

        organism.Movement.Move(organism, (Vector2)pointAhead, run);
        return true;
    }

    public void StopMovement(Organism organism)
    {
        organism.Movement.StopMovement();
    }

    public void WanderAround(AIOrganism organism, ALocation location, bool run)
    {
        if (!_wandering)
        {
            StopMovement(organism);
            location.StartWandering(organism, _patience);
            _wandering = true;
            return;
        }
        location.Wander(organism);
    }

    public void StopWandering()
    {
        _wandering = false;
    }
}
