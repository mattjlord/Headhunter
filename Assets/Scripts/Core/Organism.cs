using UnityEngine;

public class Organism : WorldObject
{
    [SerializeField] private Vitals _vitals;
    [SerializeField] private Senses _senses;
    [SerializeField] private Movement _movement;
    [SerializeField] private float _reach;

    private Vector2 _lookDirection = Vector2.up;

    public Vitals Vitals { get { return _vitals; } }
    public Senses Senses { get { return _senses; } }
    public Movement Movement {  get { return _movement; } }
    public float Reach { get { return _reach; } }

    public Vector2 LookDirection
    {
        get { return _lookDirection; }
        set
        {
            _lookDirection = value;
            float angleInRadians = Mathf.Atan2(value.x, value.y);
            float angleInDeg = Mathf.Rad2Deg * angleInRadians;
            transform.rotation = Quaternion.Euler(0, angleInDeg, 0);
        }
    }

    protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Vector3 pos = VectorUtils.Vec2ToVec3(Position);
        pos.y = 0.25f;
        Gizmos.DrawWireSphere(pos, _reach);

        Gizmos.color = Color.white;
        Vector3 worldLookDir = VectorUtils.Vec2ToVec3(_lookDirection);
        Gizmos.DrawRay(pos, worldLookDir * 5f);
    }
}
