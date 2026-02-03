using UnityEngine;

public class Senses : MonoBehaviour
{
    [SerializeField] private Transform _headTransform;
    [SerializeField] private float _sightRadius;
    [SerializeField] private float _fov;
    [SerializeField] private float _hearingRadius;
    [SerializeField] private float _smellRadius;
    public bool CanSense(AStimulus stimulus)
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

    private bool CanSee(AStimulus stimulus)
    {
        if (stimulus.SenseType != SenseType.Sight)
            return false;
        // TODO: Also block stuff that's hidden behind obstacles
        return StimulusInRange(stimulus, _sightRadius) && StimulusInFOV(stimulus);
    }

    private bool CanHear(AStimulus stimulus)
    {
        if (stimulus.SenseType != SenseType.Sound)
            return false;
        return StimulusInRange(stimulus, _hearingRadius);
    }

    private bool CanSmell(AStimulus stimulus)
    {
        if (stimulus.SenseType != SenseType.Smell) 
            return false;
        return StimulusInRange(stimulus, _smellRadius);
    }

    private bool StimulusInRange(AStimulus stimulus, float radius)
    {
        ALocation stimulusLocation = stimulus.Location;
        Vector2 headPos = VectorUtils.Vec3ToVec2(_headTransform.position);
        Vector2 nearestPoint = stimulusLocation.GetClosestPoint(headPos);

        float maxDistance = radius + stimulus.DetectableDistance;
        float distanceToStimulus = Vector2.Distance(nearestPoint, headPos);
        return distanceToStimulus <= maxDistance;
    }

    private bool StimulusInFOV(AStimulus stimulus)
    {
        ALocation stimulusLocation = stimulus.Location;

        Vector2 headPos = VectorUtils.Vec3ToVec2(_headTransform.position);
        Vector2 closestPoint = stimulusLocation.GetClosestPoint(headPos);

        Vector2 toStimulus = (closestPoint - headPos).normalized;
        Vector2 forward = VectorUtils.Vec3ToVec2(_headTransform.forward).normalized;

        float angleToStimulus = Vector2.Angle(forward, toStimulus);

        return angleToStimulus <= (_fov * 0.5f);
    }

    private void OnDrawGizmos()
    {
        Vector3 headPos = _headTransform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(headPos, _smellRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(headPos, _hearingRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(headPos, _sightRadius);

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
