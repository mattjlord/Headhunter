using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationKnowledge : MonoBehaviour
{
    [SerializeField] private List<ALocation> _foodLocations;
    [SerializeField] private List<ALocation> _waterLocations;
    [SerializeField] private List<ALocation> _shelterLocations;
    private List<ALocation> _blockedLocations;

    private void Start()
    {
        _blockedLocations = new List<ALocation>();
    }

    public ALocation? GetClosestFood(Vector2 currentPos)
    {
        float shortestDistance = Mathf.Infinity;
        ALocation closestLocation = null;

        foreach (var location in _foodLocations)
        {
            if (_blockedLocations.Contains(location))
                continue;
            float distance = location.GetDistanceFrom(currentPos);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestLocation = location;
            }
        }

        return closestLocation;
    }

    public ALocation? GetClosestWater(Vector2 currentPos)
    {
        float shortestDistance = Mathf.Infinity;
        ALocation closestLocation = null;

        foreach (var location in _waterLocations)
        {
            if (_blockedLocations.Contains(location))
                continue;
            float distance = location.GetDistanceFrom(currentPos);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestLocation = location;
            }
        }

        return closestLocation;
    }

    public ALocation? GetClosestShelter(Vector2 currentPos)
    {
        float shortestDistance = Mathf.Infinity;
        ALocation closestLocation = null;

        foreach (var location in _shelterLocations)
        {
            if (_blockedLocations.Contains(location))
                continue;
            float distance = location.GetDistanceFrom(currentPos);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestLocation = location;
            }
        }

        return closestLocation;
    }

    public void BlockLocation(ALocation location)
    {
        _blockedLocations.Add(location);
    }

    public bool IsLocationBlocked(ALocation location)
    {
        return _blockedLocations.Contains(location);
    }
}
