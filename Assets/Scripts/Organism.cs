using UnityEngine;

public class Organism : WorldObject
{
    [SerializeField] private Vitals _vitals;
    [SerializeField] private Senses _senses;
    [SerializeField] private Movement _movement;
    [SerializeField] private float _reach;

    public Vitals Vitals { get { return _vitals; } }
    public Senses Senses { get { return _senses; } }
    public Movement Movement {  get { return _movement; } }
    public float Reach { get { return _reach; } }

    protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Vector3 pos = VectorUtils.Vec2ToVec3(Position);
        pos.y = 0.25f;
        Gizmos.DrawWireSphere(pos, _reach);
    }
}
