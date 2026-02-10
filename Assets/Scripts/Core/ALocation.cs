using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ALocation : MonoBehaviour
{
    private Dictionary<AIOrganism, (float, float)> _wanderers;
    protected static float sensitivity = 1.0f;
    protected virtual void Start()
    {
        _wanderers = new Dictionary<AIOrganism, (float, float)>();
    }
    protected virtual void FixedUpdate()
    {
        List<AIOrganism> toRemove = new List<AIOrganism>();
        foreach (var entry in _wanderers)
        {
            if (Time.time > entry.Value.Item1 + entry.Value.Item2)
            {
                toRemove.Add(entry.Key);
            }
        }

        foreach(AIOrganism organism in toRemove)
        {
            RemoveWanderer(organism);
        }
    }

    private void RemoveWanderer(AIOrganism organism)
    {
        _wanderers.Remove(organism);
        organism.Navigation.StopWandering();
        organism.LocationKnowledge.BlockLocation(this);
        organism.TaskManagement.RemoveAssociatedTasks(this);
    }

    public abstract Vector2 GetClosestPoint(Vector2 point);
    public abstract float GetDistanceFrom(Vector2 point);
    public virtual bool LocationReachedByOrganism(AIOrganism organism)
    {
        return LocationReached(organism.Position);
    }
    public abstract bool LocationReached(Vector2 point);
    public void StartWandering(AIOrganism organism, float patience)
    {
        if (_wanderers.ContainsKey(organism)) { return; }
        _wanderers.Add(organism, (Time.fixedTime, patience));
    }

    public virtual void Wander(AIOrganism organism) { }

    public abstract List<Collider> GetNearbyColliders(float radius);
}
