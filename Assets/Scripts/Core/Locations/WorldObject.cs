using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] private float _radius;
    public Vector2 Position
    {
        get { return VectorUtils.Vec3ToVec2(transform.position); }
        set { transform.position = VectorUtils.Vec2ToVec3(value); } // TODO: Better position logic later for flying organisms and whatnot, this is a temporary solution
    }
    public float Radius { get { return _radius; } }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = VectorUtils.Vec2ToVec3(Position);
        pos.y = 0.25f;
        Gizmos.DrawWireSphere(pos, _radius);
    }
}
