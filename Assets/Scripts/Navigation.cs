using UnityEngine;

public class Navigation : MonoBehaviour
{
    [SerializeField] private float _patience;

    private bool _wandering = false;

    public void MoveTowards(Organism organism, Vector2 pos)
    {
        Vector2 dir = (pos - organism.Position).normalized;
        organism.Movement.Move(organism, dir);
    }

    public void MoveAwayFrom(Organism organism, Vector2 pos)
    {
        Vector2 dir = (organism.Position - pos).normalized;
        organism.Movement.Move(organism, dir);
    }

    public void StopMovement(Organism organism)
    {
        organism.Movement.Move(organism, Vector2.zero);
    }

    public void WanderAround(AIOrganism organism, ALocation location)
    {
        if (!_wandering)
        {
            StopMovement(organism);
            location.StartWandering(organism, _patience);
            _wandering = true;
        }
    }

    public void StopWandering()
    {
        _wandering = false;
    }
}
