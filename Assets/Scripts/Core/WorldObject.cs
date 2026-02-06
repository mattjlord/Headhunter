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

    public AreaLocation AreaLocation { get { return _areaLocation; } }
}
