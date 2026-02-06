using System.Collections.Generic;
using UnityEngine;

public class TrailLocation : ALocation
{
    [SerializeField] private List<Vector2> _points;
    [SerializeField] private Dictionary<AIOrganism, int> _wanderIndices;

    protected override void Start()
    {
        base.Start();
        _wanderIndices = new Dictionary<AIOrganism, int>();
        if (_points == null)
            _points = new List<Vector2>();
    }
    public override Vector2 GetClosestPoint(Vector2 point)
    {
        int index = GetClosestIndex(point);
        if (index >= 0)
        {
            return _points[index];
        }
        return point;
    }

    private int GetClosestIndex(Vector2 point)
    {
        int index = -1;
        float distance = Mathf.Infinity;

        for (int i = 0; i < _points.Count; i++)
        {
            Vector2 nextPoint = _points[i];
            float distToPoint = Vector2.Distance(point, nextPoint);
            if (distToPoint < distance)
            {
                distance = distToPoint;
                index = i;
            }
        }

        return index;
    }

    public override float GetDistanceFrom(Vector2 point)
    {
        Vector2 closestPoint = GetClosestPoint(point);
        return Vector2.Distance(point, closestPoint);
    }

    public override bool LocationReachedByOrganism(AIOrganism organism)
    {
        if (!_wanderIndices.ContainsKey(organism))
            return base.LocationReachedByOrganism(organism);
        else
            return true;
    }

    public override bool LocationReached(Vector2 point)
    {
        return Vector2.Distance(GetClosestPoint(point), point) < sensitivity;
    }

    public override void Wander(AIOrganism organism)
    {
        int nextWanderIndex = GetNextWanderIndex(organism);
        if (nextWanderIndex == -1)
        {
            organism.Navigation.StopMovement(organism);
            return;
        }
        Vector2 nextWanderPoint = _points[nextWanderIndex];
        bool doneWandering = (nextWanderIndex == _points.Count - 1) && Vector2.Distance(organism.Position, nextWanderPoint) < sensitivity;
        if (!doneWandering)
        {
            organism.Navigation.MoveTowards(organism, nextWanderPoint);
        }
        else
        {
            organism.Navigation.StopMovement(organism);
        }
    }

    public override List<Collider> GetNearbyColliders(float radius)
    {
        List<Collider> colliders = new List<Collider>();

        foreach (Vector2 point in _points)
        {
            Vector3 worldPoint = VectorUtils.Vec2ToVec3(point);
            Collider[] hits = Physics.OverlapSphere(worldPoint, radius);
            colliders.AddRange(hits);
        }

        return colliders;
    }

    public void AddPoint(Vector2 point)
    {
        _points.Add(point);
    }

    public void Reduce()
    {
        if (_points.Count > 0)
            _points.RemoveAt(0);
    }

    private int GetNextWanderIndex(AIOrganism organism)
    {
        int currentWanderIndex = -1;
        if (_wanderIndices.ContainsKey(organism)) // Organism is already wandering on the path
        {
            currentWanderIndex = _wanderIndices[organism];
        }
        else // Organism hasn't started following the path yet
        {
            currentWanderIndex = GetClosestIndex(organism.Position); // Find the nearest index on the path
        }
        if (currentWanderIndex == -1) { return currentWanderIndex; } // Something went wrong

        Vector2 currentWanderPoint = _points[currentWanderIndex]; // Convert current index to a point
        int nextWanderIndex = currentWanderIndex; // By default, assume the organism is still finding its way to the current wander point, so next = current

        if (Vector2.Distance(organism.Position, currentWanderPoint) < sensitivity) // The organism has reached the point
        {
            nextWanderIndex = Mathf.Clamp(nextWanderIndex + 1, 0, _points.Count - 1); // Increment the wander index, unless it's the last index on the path (clamp it)
        }

        _wanderIndices[organism] = nextWanderIndex; // Update the wanderer indices dictionary

        return nextWanderIndex;
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