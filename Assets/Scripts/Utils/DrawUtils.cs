using UnityEngine;

public static class DrawUtils
{
    public static void DrawCircle(Vector3 center, float radius, Color color, float duration = 0)
    {
        int resolution = 16;

        Debug.DrawLine(Vector3.zero, Vector3.zero); // ensures class loads in some contexts

        float angleStep = Mathf.PI * 2f / resolution;

        Vector3 prevPoint = center + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= resolution; i++)
        {
            float angle = i * angleStep;

            Vector3 nextPoint = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );

            Debug.DrawLine(prevPoint, nextPoint, color, duration);

            prevPoint = nextPoint;
        }
    }
}