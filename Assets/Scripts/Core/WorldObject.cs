using System;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    // TODO: Refactor all collision logic here if it becomes an issue
    // This collision system exists because movement is manual.
    // NavMeshAgents already handle collision + avoidance.
    // When agents are introduced:
    // - disable manual collision for AI
    // - let NavMeshAgent control transform movement

    public event Action<WorldObject> OnPositionUpdate;

    [SerializeField] private AreaLocation _areaLocation; // Optional, for regional objects like grass and bodies of water
    [SerializeField] private LayerMask _collisionLayers;

    private CapsuleCollider _collider;

    private float _posUpdateRate = 5f;
    private float _lastPosUpdate;

    protected virtual void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _lastPosUpdate = Time.time;
    }

    public Vector2 Position
    {
        get { return VectorUtils.Vec3ToVec2(transform.position); }
        set
        {
            Vector3 targetPos = VectorUtils.Vec2ToVec3(value);
            if (_collider != null)
            {
                // TODO: Clean up collision later
                if (WouldCollideAt(targetPos))
                {
                    Debug.Log("Cannot move: collision detected!");
                    return; // Don't set position
                }
            }

            transform.position = targetPos;

            if (Time.time > _lastPosUpdate + _posUpdateRate)
            {
                OnPositionUpdate?.Invoke(this);
                _lastPosUpdate = Time.time;
            }
        }
    }

    public float Radius => _collider ? _collider.radius : 0;

    public AreaLocation AreaLocation => _areaLocation;

    public bool WithinReach(Vector2 position, float reach)
    {
        float dist = Vector2.Distance(Position, position);

        return dist <= (reach + Radius);
    }

    protected bool WouldCollideAt(Vector3 targetPos)
    {
        if (_collider == null) return false;

        Vector3 up = transform.up;
        float scaledHeight = _collider.height * transform.lossyScale.y;
        float scaledRadius = _collider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);

        Vector3 center = targetPos + transform.rotation * _collider.center;
        float cylinderHeight = Mathf.Max(0f, scaledHeight - 2f * scaledRadius); // cannot be negative
        Vector3 point1 = center + up * (cylinderHeight / 2f);
        Vector3 point2 = center - up * (cylinderHeight / 2f);

        Collider[] hits = Physics.OverlapCapsule(point1, point2, scaledRadius, _collisionLayers);
        foreach (var hit in hits)
        {
            if (hit != _collider) // ignore self
                return true;
        }

        return false;
    }

    public bool IsPositionValid()
    {
        return !WouldCollideAt(Position);
    }

    public virtual void OnInteraction(PlayerController playerController) { }

    public virtual string GetInteractionPhrase() { return ""; }
}
