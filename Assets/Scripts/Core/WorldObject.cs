using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private AreaLocation _areaLocation; // Optional, for regional objects like grass and bodies of water

    public Vector2 Position
    {
        get { return VectorUtils.Vec3ToVec2(transform.position); }
        set { transform.position = VectorUtils.Vec2ToVec3(value); } // TODO: Better position logic later for flying organisms and whatnot, this is a temporary solution
    }

    public float Radius
    {
        get
        {
            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            if (collider)
            {
                return collider.radius;
            }
            else
            {
                return 0;
            }
        }
    }

    public AreaLocation AreaLocation { get { return _areaLocation; } }

    public bool WithinReach(Vector2 position, float reach)
    {
        float dist = Vector2.Distance(Position, position);

        return dist <= (reach + _radius);
    }
}
