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
}