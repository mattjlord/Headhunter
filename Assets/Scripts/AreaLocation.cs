using System.Collections.Generic;
using UnityEngine;

public class AreaLocation : ALocation
{
    [SerializeField] private List<Vector2> _points;

    public void Update()
    {
        if (IsConcave())
        {
            throw new System.Exception("Concave AreaLocation!");
        }
        base.Update();
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