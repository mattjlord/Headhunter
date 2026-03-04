using UnityEngine;
using UnityEngine.AI;

public class Senses : MonoBehaviour
{
    [SerializeField] private LayerMask _visionBlocking;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private float _sightRadius;
    [SerializeField] private float _fov;
    [SerializeField] private float _hearingRadius;
    [SerializeField] private float _smellRadius;

    public bool CanSense(Stimulus stimulus)
    {
        switch (stimulus.SenseType)
        {
            case SenseType.Sight:
                return CanSee(stimulus);
            case SenseType.Sound:
                return CanHear(stimulus);
            default:
                return CanSmell(stimulus);
        }
    }

    public Vector2? GetPointAhead(float radius, OrganismType organismType)
    {
        Vector2 point = VectorUtils.Vec3ToVec2(_headTransform.position + (_headTransform.forward * _sightRadius));
        bool hasLOS = HasLineOfSight(point, out RaycastHit hit, organismType, radius);

        if (hasLOS)
        {
            DrawDebugSphere(point, radius);
            return point;
        }

        // Try to find the nearest point nearby
        const int angleStep = 15;       // degrees per check
        const int maxAngle = 90;        // maximum rotation left/right

        for (int angle = angleStep; angle <= maxAngle; angle += angleStep)
        {
            Vector3 leftDir = Quaternion.Euler(0, -angle, 0) * _headTransform.forward;
            Vector2 leftPoint = VectorUtils.Vec3ToVec2(_headTransform.position + leftDir * _sightRadius);
            if (HasLineOfSight(leftPoint, out hit, organismType, radius))
            {
                DrawDebugSphere(leftPoint, radius);
                return leftPoint;
            }

            Vector3 rightDir = Quaternion.Euler(0, angle, 0) * _headTransform.forward;
            Vector2 rightPoint = VectorUtils.Vec3ToVec2(_headTransform.position + rightDir * _sightRadius);
            if (HasLineOfSight(rightPoint, out hit, organismType, radius))
            {
                DrawDebugSphere(rightPoint, radius);
                return rightPoint;
            }
        }

        return null; // Nothing ahead
    }

    private void DrawDebugSphere(Vector2 point, float radius)
    {
        Vector3 dir = VectorUtils.Vec2ToVec3(point) - _headTransform.position;
        Debug.DrawRay(_headTransform.position + _headTransform.right * radius, dir, Color.green);
        Debug.DrawRay(_headTransform.position - _headTransform.right * radius, dir, Color.green);
    }

    private bool CanSee(Stimulus stimulus)
    {
        if (stimulus.SenseType != SenseType.Sight)
            return false;
        return StimulusInRange(stimulus, _sightRadius) && StimulusInFOV(stimulus) && StimulusUnblocked(stimulus);
    }

    public bool HasLineOfSight(Vector2 point, out RaycastHit obstacle, OrganismType? organismType = null, float radius = 0f)
    {
        obstacle = default;
        Vector2 dir = (point - VectorUtils.Vec3ToVec2(_headTransform.position));
        bool blocked = false;
        if (radius == 0)
            blocked = Physics.Raycast(_headTransform.position, VectorUtils.Vec2ToVec3(dir), out obstacle, dir.magnitude, _visionBlocking);
        else
            blocked = Physics.SphereCast(_headTransform.position, radius, VectorUtils.Vec2ToVec3(dir), out obstacle, dir.magnitude, _visionBlocking);

        bool inBounds = true;

        if (organismType != null)
        {
            NavMeshHit hit;
            int agentID = NavUtils.GetNavMeshID((OrganismType)organismType);
            NavMeshQueryFilter filter = new NavMeshQueryFilter
            {
                agentTypeID = agentID,
                areaMask = NavMesh.AllAreas
            };

            Vector3 origin = _headTransform.position;
            origin.y = 0;

            inBounds = !NavMesh.Raycast(origin, VectorUtils.Vec2ToVec3(point), out hit, filter);
        }

        return !blocked && inBounds;
    }

    private bool CanHear(Stimulus stimulus)
    {
        if (stimulus.SenseType != SenseType.Sound)
            return false;
        return StimulusInRange(stimulus, _hearingRadius);
    }

    private bool CanSmell(Stimulus stimulus)
    {
        if (stimulus.SenseType != SenseType.Smell) 
            return false;
        return StimulusInRange(stimulus, _smellRadius);
    }

    private bool StimulusInRange(Stimulus stimulus, float radius)
    {
        ALocation stimulusLocation = stimulus.Location;
        Vector2 headPos = VectorUtils.Vec3ToVec2(_headTransform.position);
        Vector2? nearestPoint = stimulusLocation.GetClosestPoint(headPos, OrganismType.Hunter);

        if (nearestPoint == null) return false;

        float maxDistance = radius + stimulus.DetectableDistance;
        float distanceToStimulus = Vector2.Distance((Vector2)nearestPoint, headPos);
        return distanceToStimulus <= maxDistance;
    }

    private bool StimulusInFOV(Stimulus stimulus)
    {
        ALocation stimulusLocation = stimulus.Location;

        Vector2 headPos = VectorUtils.Vec3ToVec2(_headTransform.position);
        Vector2? closestPoint = stimulusLocation.GetClosestPoint(headPos, OrganismType.Hunter);

        if (closestPoint == null) return false;

        Vector2 toStimulus = ((Vector2)closestPoint - headPos).normalized;
        Vector2 forward = VectorUtils.Vec3ToVec2(_headTransform.forward).normalized;

        float angleToStimulus = Vector2.Angle(forward, toStimulus);

        return angleToStimulus <= (_fov * 0.5f);
    }

    private bool StimulusUnblocked(Stimulus stimulus)
    {
        ALocation stimulusLocation = stimulus.Location;

        Vector2 headPos = VectorUtils.Vec3ToVec2(_headTransform.position);
        Vector2? closestPoint = stimulusLocation.GetClosestPoint(headPos, OrganismType.Hunter);

        if (closestPoint == null) return false;

        return HasLineOfSight((Vector2)closestPoint, out RaycastHit hit);
    }

    private void OnDrawGizmos()
    {
        Vector3 headPos = _headTransform.position;

        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(headPos, _smellRadius);
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(headPos, _hearingRadius);
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(headPos, _sightRadius);

        // FOV
        Vector3 forward = _headTransform.forward;
        float halfFOV = _fov * 0.5f;

        Vector3 leftBoundary =
            Quaternion.Euler(0f, -halfFOV, 0f) * forward;
        Vector3 rightBoundary =
            Quaternion.Euler(0f, halfFOV, 0f) * forward;

        Gizmos.DrawRay(headPos, leftBoundary.normalized * _sightRadius);
        Gizmos.DrawRay(headPos, rightBoundary.normalized * _sightRadius);
    }
}
