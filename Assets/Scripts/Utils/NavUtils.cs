using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;

public static class NavUtils
{

    public static int GetNavMeshID(OrganismType organismType)
    {
        switch (organismType)
        {
            case OrganismType.Mudyak:
                return -1372625422;
            case OrganismType.BulletRaptor:
                return -334000983;
            default:
                return 0;
        }
    }

    public static bool LineInBounds(OrganismType organismType, Vector2 start, Vector2 end)
    {
        NavMeshHit hit;
        int agentID = GetNavMeshID(organismType);
        NavMeshQueryFilter filter = new NavMeshQueryFilter
        {
            agentTypeID = agentID,
            areaMask = NavMesh.AllAreas
        };

        return !NavMesh.Raycast(VectorUtils.Vec2ToVec3(start), VectorUtils.Vec2ToVec3(end), out hit, filter);
    }

    public static bool PointInBounds(OrganismType organismType, Vector2 point, float maxDistance, out NavMeshHit hit)
    {
        int agentID = GetNavMeshID(organismType);
        NavMeshQueryFilter filter = new NavMeshQueryFilter
        {
            agentTypeID = agentID,
            areaMask = NavMesh.AllAreas
        };

        return NavMesh.SamplePosition(VectorUtils.Vec2ToVec3(point), out hit, maxDistance, filter);
    }

    public static Vector2 AdjustVelocity(Organism thisOrganism, Vector2 thisVelocity)
    {
        float thisRadius = thisOrganism.Radius;
        Vector2 thisPosition = thisOrganism.Position;
        Vector2 thisRight = new Vector2(thisOrganism.LookDirection.y, -thisOrganism.LookDirection.x).normalized;
        Vector2 thisRightSide = thisPosition + (thisRadius * thisRight);
        Vector2 thisLeftSide = thisPosition - (thisRadius * thisRight);

        List<(Vector2, Vector2)> allIntersections = new List<(Vector2, Vector2)>();

        foreach (Organism organism in MasterOrganismManager.AllOrganisms)
        {
            if (organism == thisOrganism) { continue; }
            Vector2 thatPosition = organism.Position;
            float thatRadius = organism.Radius;
            Vector2 thatVelocity = organism.Movement.Velocity;// + (organism.LookDirection * thatRadius);
            Vector2 thatRight = new Vector2(organism.LookDirection.y, -organism.LookDirection.x).normalized;
            Vector2 thatRightSide = thatPosition + (thatRadius * thatRight);
            Vector2 thatLeftSide = thatPosition - (thatRadius * thatRight);

            Vector2 rightRightIntersection;
            Vector2 rightLeftIntersection;
            Vector2 leftRightIntersection;
            Vector2 leftLeftIntersection;

            List<Vector2> intersections = new List<Vector2>();

            // Right-side intersections
            bool rightRight = VectorUtils.DoRaysIntersect(thisRightSide, thisVelocity, thatRightSide, thatVelocity, out rightRightIntersection);
            bool rightLeft = VectorUtils.DoRaysIntersect(thisRightSide, thisVelocity, thatLeftSide, thatVelocity, out rightLeftIntersection);

            if (rightRight)
                intersections.Add(rightRightIntersection - (thisRadius * thisRight));
            if (rightLeft)
                intersections.Add(rightLeftIntersection - (thisRadius * thisRight));

            // Left-side intersections
            bool leftRight = VectorUtils.DoRaysIntersect(thisLeftSide, thisVelocity, thatRightSide, thatVelocity, out leftRightIntersection);
            bool leftLeft = VectorUtils.DoRaysIntersect(thisLeftSide, thisVelocity, thatLeftSide, thatVelocity, out leftLeftIntersection);

            if (leftRight)
                intersections.Add(leftRightIntersection + (thisRadius * thisRight));
            if (leftLeft)
                intersections.Add(leftLeftIntersection + (thisRadius * thisRight));

            Vector2? closestIntersection = null;
            float shortestDistance = Mathf.Infinity;

            foreach (Vector2 intersection in intersections)
            {
                float distance = Vector2.Distance(thisPosition, intersection);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestIntersection = intersection;
                }
            }

            if (closestIntersection != null)
            {
                allIntersections.Add(((Vector2)closestIntersection, thatVelocity));
            }
        }

        if (allIntersections.Count == 0)
            return thisVelocity; // nothing to avoid

        Vector2 totalAvoidance = Vector2.zero;

        foreach (var (intersection, otherVel) in allIntersections)
        {
            Vector3 debugPoint = VectorUtils.Vec2ToVec3(intersection) + Vector3.up;
            Vector3 debugThis = VectorUtils.Vec2ToVec3(thisPosition) + Vector3.up;
            DrawUtils.DrawCircle(debugPoint, thisRadius, Color.red);
            Debug.DrawRay(debugPoint, VectorUtils.Vec2ToVec3(otherVel), Color.red);
            Debug.DrawRay(debugPoint, 4 * Vector3.up, Color.red);
            Debug.DrawLine(debugThis + 4 * Vector3.up, debugPoint + 4 * Vector3.up, Color.red);

            float dist = Vector2.Distance(thisOrganism.Position, intersection);
            float weight = Mathf.Clamp01((thisRadius * 2f - dist) / (thisRadius * 2f));
            //weight = 1f;
            Vector2 away = (thisPosition - intersection).normalized * weight;

            Vector2 slide = (otherVel.sqrMagnitude > 0.001f) ? Vector2.Perpendicular(otherVel).normalized * weight * 0.5f : Vector2.zero;

            Debug.DrawRay(debugThis, 4 * Vector3.up, Color.red);
            Debug.DrawRay(debugThis, VectorUtils.Vec2ToVec3(away), Color.cyan);
            Debug.DrawRay(debugThis, VectorUtils.Vec2ToVec3(slide), Color.magenta);

            totalAvoidance += away + slide;
        }

        if (totalAvoidance.sqrMagnitude < 0.001f)
            return thisVelocity.normalized; // nothing to avoid

        float avoidanceStrength = 0.3f; // tweak for smooth steering
        Vector2 blended = Vector2.Lerp(thisVelocity.normalized, totalAvoidance.normalized, avoidanceStrength).normalized;

        return blended;
    }
}