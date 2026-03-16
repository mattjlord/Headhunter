using UnityEngine;

public static class VectorUtils
{
    public static Vector3 Vec2ToVec3(Vector2 vec2)
    {
        return new Vector3(vec2.x, 0, vec2.y);
    }

    public static Vector2 Vec3ToVec2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.z);
    }

    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public static bool PointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        float cross = (p.y - a.y) * (b.x - a.x) - (p.x - a.x) * (b.y - a.y);
        if (Mathf.Abs(cross) > 0.0001f)
            return false;

        float dot = (p.x - a.x) * (b.x - a.x) + (p.y - a.y) * (b.y - a.y);
        if (dot < 0)
            return false;

        float lenSq = (b - a).sqrMagnitude;
        if (dot > lenSq)
            return false;

        return true;
    }

    public static Vector2 ClosestPointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float abSqr = ab.sqrMagnitude;
        if (abSqr == 0f) return a; // a and b are the same point

        float t = Vector2.Dot(p - a, ab) / abSqr;
        t = Mathf.Clamp01(t);

        return a + t * ab;
    }

    public static bool DoRaysIntersect(Vector2 ray1Origin, Vector2 ray1Dir, Vector2 ray2Origin, Vector2 ray2Dir, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        float cross = ray1Dir.x * ray2Dir.y - ray1Dir.y * ray2Dir.x;

        // Parallel rays
        if (Mathf.Abs(cross) == 0.00001f)
            return false;

        Vector2 diff = ray2Origin - ray1Origin;

        float t = (diff.x * ray2Dir.y - diff.y * ray2Dir.x) / cross;
        float u = (diff.x * ray1Dir.y - diff.y * ray1Dir.x) / cross;

        // Ensure intersection is forward along both rays
        if (t >= 0f && u >= 0f)
        {
            intersection = ray1Origin + ray1Dir * t;
            return true;
        }

        return false;
    }
}