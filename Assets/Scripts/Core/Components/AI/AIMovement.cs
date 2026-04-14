using UnityEngine;
using UnityEngine.AI;

public class AIMovement : AMovement
{
    private NavMeshAgent _agent;

    public override void StopMovement()
    {
        dir = Vector2.zero;
        if (_agent.enabled)
            _agent.isStopped = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Move(Vector2 value)
    {
        if (!_agent.enabled && value != Vector2.zero)
            _agent.enabled = true;
        else if (!_agent.enabled)
            return;
        _agent.isStopped = false;
        _agent.SetDestination(VectorUtils.Vec2ToVec3(value));
        UpdateDir();
    }

    protected override void UpdateMove(float speed)
    {
        _agent.speed = speed;
        UpdateDir();
    }

    private void UpdateDir()
    {
        NavMeshPath path = _agent.path;
        if (path.corners.Length > 1)
        {
            dir = (VectorUtils.Vec3ToVec2(path.corners[1]) - organism.Position).normalized;
            //organism.ActionManagement.Stop();
        }
        else
            dir = Vector2.zero;
    }

    public override void HideFromNavMesh()
    {
        _agent.enabled = false;
    }
}
