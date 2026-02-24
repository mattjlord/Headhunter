using UnityEngine;

public enum ControlState
{
    World,
    Menu
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerOrganism _organism;
    [SerializeField] private InventoryUI _inventoryUI;

    private ControlState _controlState = ControlState.World;

    private bool _isZooming;

    private Vector2 _lookDirection = Vector2.up;
    private Vector2 _lookPoint;

    private bool _isRunning = false;
    private Vector2 _moveDirection = Vector2.zero;

    private void Update()
    {
        // TODO: Optimize menu code once there is more than just one menu type

        switch(_controlState)
        {
            case ControlState.World:
                WhileInWorldState();
                break;
            case ControlState.Menu:
                WhileInMenuState();
                break;
            default:
                break;
        }

        ParseAndUpdateMenuControls();
    }

    private void WhileInWorldState()
    {
        ParseZooming();
        ParseLookDirection();
        ParseMovement();

        UpdateLookDirection();
        UpdateMovement();
        UpdateCamera();

        ParseAndUpdateShooting();
    }

    private void WhileInMenuState()
    {
        
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
        // TODO: Dynamic camera controls
    }

    private void ParseAndUpdateShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _organism.Shooting.Shoot(_organism.LookDirection);
        }
    }

    private void ParseAndUpdateMenuControls()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventoryUI.Enabled = !_inventoryUI.Enabled;
            if (_inventoryUI.Enabled)
            {
                _controlState = ControlState.Menu;
                _organism.Movement.Move(_organism, Vector2.zero, false);
            }
            else
                _controlState = ControlState.World;
        }
    }
}
