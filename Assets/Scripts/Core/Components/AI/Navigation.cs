using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class Navigation : MonoBehaviour
{
    [SerializeField] private float _patience;

    private bool _wandering = false;

    private Vector2 _currentDestination;
    private NavMeshPath _path;
    private int _pathIndex;

    private void Awake()
    {
        _path = new NavMeshPath();
    }

    public void MoveTowards(Organism organism, Vector2 pos, bool run, bool chasing = false)
    {
        Vector2 dir;

        if (chasing)
        {
            dir = (pos - organism.Position).normalized;
        }
        else
        {
            dir = GetNavMeshDir(organism, pos);
        }

        Debug.DrawLine(transform.position + Vector3.up, VectorUtils.Vec2ToVec3(pos) + Vector3.up, Color.cyan);

        organism.Movement.Move(organism, dir, run);
    }

    Vector2 GetNavMeshDir(Organism organism, Vector2 pos)
    {
        if (_currentDestination != pos)
        {
            InitPath(organism, pos);
        }

        if (_path.corners.Length == 0)
        {
            Debug.DrawRay(transform.position, Vector3.up * 10, Color.red);
            Debug.DrawLine(transform.position + (Vector3.up * 10), VectorUtils.Vec2ToVec3(pos) + (Vector3.up * 10), Color.red);
            Debug.DrawRay(VectorUtils.Vec2ToVec3(pos), Vector3.up * 10, Color.red);
            return Vector2.zero;
        }

        Debug.DrawRay(transform.position, Vector3.up * 10, Color.green);

        if (_pathIndex >= _path.corners.Length)
            return Vector2.zero;

        Vector3 nextPoint = _path.corners[_pathIndex];

        Debug.DrawLine(transform.position + Vector3.up, nextPoint + Vector3.up, Color.blue);

        Vector2 nextPoint2D = VectorUtils.Vec3ToVec2(nextPoint);

        float distToNextPoint = Vector2.Distance(organism.Position, nextPoint2D);
        if (distToNextPoint < organism.Movement.CurrentSpeed)
        {
            _pathIndex++;
        }

        return (nextPoint2D - organism.Position).normalized;
    }

    public void MoveAwayFrom(Organism organism, Vector2 pos, bool run)
    {
        // TODO: Fix this shitty code it barely works and looks terrible in-game
        Vector2 lookDir = (organism.Position - pos).normalized;
        organism.LookDirection = lookDir;

        Vector2? pointAhead = organism.Senses.GetPointAhead(organism.Radius, organism.OrganismType);

        if (pointAhead == null)
        {
            // TODO: Somehow integrate behavior for when the organism gets cornered
            return;
        }

        Vector2 moveDir = ((Vector2)pointAhead - organism.Position).normalized;

        organism.Movement.Move(organism, moveDir, run);
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
        _path.ClearCorners();
    }

    private void InitPath(Organism organism, Vector2 destination)
    {
        _currentDestination = destination;
        _pathIndex = 1;
        int agentID = NavUtils.GetNavMeshID(organism.OrganismType);
        NavMeshQueryFilter filter = new NavMeshQueryFilter
        {
            agentTypeID = agentID,
            areaMask = NavMesh.AllAreas
        };
        NavMesh.CalculatePath(VectorUtils.Vec2ToVec3(organism.Position), VectorUtils.Vec2ToVec3(destination), filter, _path);
    }

    private void OnDrawGizmos()
    {
        if (_path == null || _path.corners.Length == 0)
            return;

        Vector3? lastCorner = null;

        foreach(var corner in _path.corners)
        {
            if (lastCorner != null)
                Gizmos.DrawLine((Vector3)lastCorner, corner);
            lastCorner = corner;
        }
    }
}
