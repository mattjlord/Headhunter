using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerOrganism _organism;

    private bool _isZooming;

    private Vector2 _lookDirection = Vector2.up;
    private Vector2 _lookPoint;

    private bool _isRunning = false;
    private Vector2 _moveDirection = Vector2.zero;

    private void Update()
    {
        ParseZooming();
        ParseLookDirection();
        ParseMovement();

        UpdateLookDirection();
        UpdateMovement();
        UpdateCamera();
    }

    private void ParseZooming()
    {
        _isZooming = Input.GetKey(KeyCode.Mouse1);
    }

    private void ParseLookDirection()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 lookPoint3D = ray.GetPoint(distance);
            Vector2 worldPoint2D = VectorUtils.Vec3ToVec2(lookPoint3D);
            _lookDirection = (worldPoint2D - _organism.Position).normalized;
            _lookPoint = worldPoint2D;
        }
    }

    private void ParseMovement()
    {
        _isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector2 moveDirection = Vector2.zero;

        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        if (up && !down) { moveDirection.y = 1; }
        if (down && !up) { moveDirection.y = -1; }
        if (left && !right) { moveDirection.x = -1; }
        if (right && !left) { moveDirection.x = 1; }

        _moveDirection = moveDirection.normalized;
    }

    private void UpdateLookDirection()
    {
        _organism.LookDirection = _lookDirection;
    }

    private void UpdateMovement()
    {
        _organism.Movement.Move(_organism, _moveDirection, _isRunning);
    }

    private void UpdateCamera()
    {

    }
}
