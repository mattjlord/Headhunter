using UnityEngine;

public class Navigation : MonoBehaviour
{
    [SerializeField] private float _patience;

    private bool _wandering = false;

    public void MoveTowards(Organism organism, Vector2 pos, bool run)
    {
        Vector2 dir = (pos - organism.Position).normalized;
        organism.Movement.Move(organism, dir, run);
    }

    public void MoveAwayFrom(Organism organism, Vector2 pos, bool run)
    {
        Vector2 dir = (organism.Position - pos).normalized;
        organism.Movement.Move(organism, dir, run);
    }

    public void StopMovement(Organism organism)
    {
        organism.Movement.Move(organism, Vector2.zero, false);
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
