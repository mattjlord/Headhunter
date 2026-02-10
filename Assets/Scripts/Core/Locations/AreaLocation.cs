using System.Collections.Generic;
using UnityEngine;

public class AreaLocation : ALocation
{
    [SerializeField] private List<Vector2> _points;
    private Dictionary<AIOrganism, Vector2> _wanderDestinations;

    protected override void Start()
    {
        base.Start();
        _wanderDestinations = new Dictionary<AIOrganism, Vector2>();
    }

    protected override void FixedUpdate()
    {
        if (IsConcave())
        {
            throw new System.Exception("Concave AreaLocation!");
        }
        base.FixedUpdate();
    }

    private Vector2 WorldSpacePoint(int index)
    {
        Vector3 local = VectorUtils.Vec2ToVec3(_points[index]);
        Vector3 world = transform.TransformPoint(local);
        return VectorUtils.Vec3ToVec2(world);
    }

    public override Vector2 GetClosestPoint(Vector2 point)
    {
        if (_points == null || _points.Count < 2)
            return point;

        if (LocationReached(point))
            return point;

        Vector2 closest = WorldSpacePoint(0);
        float minDistSq = float.MaxValue;
        int count = _points.Count;

        for (int i = 0; i < count; i++)
        {
            Vector2 a = WorldSpacePoint(i);
            Vector2 b = WorldSpacePoint((i + 1) % count);

            Vector2 candidate = VectorUtils.ClosestPointOnSegment(point, a, b);
            float distSq = (point - candidate).sqrMagnitude;

            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closest = candidate;
            }
        }

        return closest;
    }

    public override float GetDistanceFrom(Vector2 point)
    {
        Vector2 closestPoint = GetClosestPoint(point);
        return Vector2.Distance(closestPoint, point);
    }

    public override bool LocationReached(Vector2 point)
    {
        if (_points == null || _points.Count < 3)
            return false;

        int count = _points.Count;

        for (int i = 0; i < count; i++)
        {
            Vector2 a = WorldSpacePoint(i);
            Vector2 b = WorldSpacePoint((i + 1) % count);

            if (VectorUtils.PointOnSegment(point, a, b))
                return true;
        }

        bool inside = false;
        for (int i = 0, j = count - 1; i < count; j = i++)
        {
            Vector2 pi = WorldSpacePoint(i);
            Vector2 pj = WorldSpacePoint(j);

            bool intersects =
                ((pi.y > point.y) != (pj.y > point.y)) &&
                (point.x < (pj.x - pi.x) * (point.y - pi.y) / (pj.y - pi.y) + pi.x);

            if (intersects)
                inside = !inside;
        }

        return inside;
    }

    private bool IsConcave()
    {
        if (_points == null || _points.Count < 4)
            return false;

        bool? sign = null;

        int count = _points.Count;

        for (int i = 0; i < count; i++)
        {
            Vector2 a = WorldSpacePoint(i);
            Vector2 b = WorldSpacePoint((i + 1) % count);
            Vector2 c = WorldSpacePoint((i + 2) % count);

            Vector2 ab = b - a;
            Vector2 bc = c - b;

            float cross = VectorUtils.Cross(ab, bc);

            if (Mathf.Abs(cross) < Mathf.Epsilon)
                continue;

            bool currentSign = cross > 0f;

            if (sign == null)
            {
                sign = currentSign;
            }
            else if (sign != currentSign)
            {
                return true;
            }
        }

        return false;
    }

    public override void Wander(AIOrganism organism)
    {
        Vector2 wanderDestination = GetOrUpdateWanderDestination(organism);
        organism.Navigation.MoveTowards(organism, wanderDestination);
    }

    private Vector2 GetOrUpdateWanderDestination(AIOrganism organism)
    {
        if (_wanderDestinations.ContainsKey(organism))
        {
            float distance = Vector2.Distance(organism.Position, _wanderDestinations[organism]);
            if (distance >= sensitivity)
            {
                return _wanderDestinations[organism];
            }
        }
        Vector2 wanderDestination = GetRandomPointInArea();
        _wanderDestinations[organism] = wanderDestination;
        return wanderDestination;
    }

    private Vector2 GetRandomPointInArea()
    {
        if (_points == null || _points.Count < 3)
            return Vector2.zero;

        (Vector2, Vector2) boundingBox = GetBoundingBox();

        float minX = boundingBox.Item1.x, maxX = boundingBox.Item2.x;
        float minY = boundingBox.Item1.y, maxY = boundingBox.Item2.y;

        while (true)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            Vector2 candidate = new Vector2(x, y);

            if (LocationReached(candidate))
                return candidate;
        }
    }

    public override List<Collider> GetNearbyColliders(float radius)
    {
        (Vector2, float) boundingCircle = GetBoundingCircle();
        Vector3 boundingCenter = VectorUtils.Vec2ToVec3(boundingCircle.Item1);
        float boundingRadius = boundingCircle.Item2;

        Collider[] hits = Physics.OverlapSphere(boundingCenter, boundingRadius + radius);

        return new List<Collider>(hits);
    }

    private (Vector2, Vector2) GetBoundingBox()
    {
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        foreach (var p in _points)
        {
            Vector2 wp = WorldSpacePoint(_points.IndexOf(p));
            if (wp.x < minX) minX = wp.x;
            if (wp.x > maxX) maxX = wp.x;
            if (wp.y < minY) minY = wp.y;
            if (wp.y > maxY) maxY = wp.y;
        }

        return (new Vector2(minX, minY), new Vector2(maxX, maxY));
    }

    private (Vector2, float) GetBoundingCircle()
    {
        (Vector2, Vector2) boundingBox = GetBoundingBox();

        float minX = boundingBox.Item1.x, maxX = boundingBox.Item2.x;
        float minY = boundingBox.Item1.y, maxY = boundingBox.Item2.y;

        float centerX = (minX + maxX) / 2;
        float centerY = (minY + maxY) / 2;

        Vector2 center = new Vector2(centerX, centerY);

        float radius = 0;

        for (int i = 0; i < _points.Count; i++)
        {
            Vector2 point = WorldSpacePoint(i);
            float dist = Vector2.Distance(point, center);
            if (dist > radius) radius = dist;
        }

        return (center, radius);
    }

    void OnDrawGizmos()
    {
        if (_points.Count == 0)
            return;
        Vector3 startPoint = VectorUtils.Vec2ToVec3(_points[0]);
        startPoint = transform.TransformPoint(startPoint);
        startPoint.y = 0.25f;
        Vector3? prevPoint = null;
        if (!IsConcave())
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        foreach (Vector2 vec2 in _points)
        {
            Vector3 vec3 = VectorUtils.Vec2ToVec3(vec2);
            vec3 = transform.TransformPoint(vec3);
            vec3.y = 0.25f; // To prevent clipping through ground
            if (prevPoint != null)
                Gizmos.DrawLine((Vector3)prevPoint, vec3);
            prevPoint = vec3;
        }
        Gizmos.DrawLine((Vector3)prevPoint, startPoint);
    }
}