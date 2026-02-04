using System.Collections.Generic;
using UnityEngine;

public class PointLocation : ALocation
{
    public override Vector2 GetClosestPoint(Vector2 point)
    {
        return VectorUtils.Vec3ToVec2(transform.position);
    }

    public override float GetDistanceFrom(Vector2 point)
    {
        return Vector2.Distance(transform.position, point);
    }

    public override bool LocationReached(Vector2 point)
    {
        return Vector2.Distance(VectorUtils.Vec3ToVec2(transform.position), point) < sensitivity;
    }

    public override List<Collider> GetNearbyColliders(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        return new List<Collider>(hits);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 pos = transform.position;
        pos.y = 0.25f;
        Gizmos.DrawWireSphere(pos, 0.5f);
    }
}