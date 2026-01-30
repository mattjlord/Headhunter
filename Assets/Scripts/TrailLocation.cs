using System.Collections.Generic;
using UnityEngine;

public class TrailLocation : ALocation
{
    [SerializeField] private List<Vector2> _points;
    public override Vector2 GetClosestPoint(Vector2 point)
    {
        Vector2 closestPoint = _points[0];
        float distance = Mathf.Infinity;

        foreach (Vector2 p in _points)
        {
            float distToPoint = Vector2.Distance(point, p);
            if (distToPoint < distance)
            {
                distance = distToPoint;
                closestPoint = p;
            }
        }

        return closestPoint;
    }

    public override float GetDistanceFrom(Vector2 point)
    {
        Vector2 closestPoint = GetClosestPoint(point);
        return Vector2.Distance(point, closestPoint);
    }
    public override bool LocationReached(Vector2 point)
    {
        return Vector2.Distance(GetClosestPoint(point), point) < sensitivity;
    }

    private void OnDrawGizmos()
    {
        if (_points.Count == 0)
            return;
        Vector3 startPoint = VectorUtils.Vec2ToVec3(_points[0]);
        startPoint.y = 0.25f;
        Vector3? prevPoint = null;
         Gizmos.color = Color.green;
        foreach (Vector2 vec2 in _points)
        {
            Vector3 vec3 = VectorUtils.Vec2ToVec3(vec2);
            vec3.y = 0.25f; // To prevent clipping through ground
            if (prevPoint != null)
                Gizmos.DrawLine((Vector3)prevPoint, vec3);
            prevPoint = vec3;
        }

        Vector3 endPoint = (Vector3)prevPoint; 
        endPoint.y = 0.25f;
        Gizmos.DrawWireSphere(endPoint, 0.5f);
    }
}