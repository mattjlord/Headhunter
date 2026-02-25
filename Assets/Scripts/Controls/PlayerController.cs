using UnityEngine;

public enum ControlState
{
    World,
    Menu
}

// TODO: Improve state handling all around - this is messy but it works
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerOrganism _organism;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private ContainerUI _scavengingUI;

    private ControlState _controlState = ControlState.World;

    private Vector2 _lookDirection = Vector2.up;
    private Vector2 _lookPoint;

    private bool _isRunning = false;
    private Vector2 _moveDirection = Vector2.zero;

    private Container _openContainer = null;

    public void OpenContainer(Container container)
    {
        _scavengingUI.Container = container;
        _scavengingUI.Enabled = true;
        _openContainer = container;
        OnOpenAnyMenu();
    }

    private void CloseContainer()
    {
        _scavengingUI.Container = null;
        _scavengingUI.Enabled = false;
        _openContainer = null;
    }

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
        ParseLookDirection();
        ParseMovement();

        UpdateLookDirection();
        UpdateMovement();

        ParseAndUpdateShooting();
        ParseAndUpdateInteraction();
    }

    private void WhileInMenuState()
    {
        ParseAndUpdateMenuClose();
        ParseAndUpdateItemInteraction();
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

    private void ParseAndUpdateShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _organism.Shooting.Shoot(_organism.LookDirection);
        }
    }

    private void ParseAndUpdateInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 worldLookPoint = VectorUtils.Vec2ToVec3(_lookPoint);
            bool objectHit = Physics.Raycast(worldLookPoint + 20 * Vector3.up, Vector3.down * 20f, out RaycastHit hitInfo);

            if (!objectHit) { return; }

            WorldObject obj = hitInfo.collider.gameObject.GetComponent<WorldObject>();

            if (obj == null) { return; }

            if (!obj.WithinReach(_organism.Position, _organism.Reach)) { return; }

            Debug.DrawRay(worldLookPoint + 20 * Vector3.up, Vector3.down * 20f, Color.red, 1f);

            obj.OnInteraction(this);
        }
    }

    private void ParseAndUpdateMenuControls()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_inventoryUI.Enabled)
            {
                if (!_scavengingUI.Enabled)
                    OnCloseAnyMenu();
                CloseInventory();
            }
            else
            {
                if (!_scavengingUI.Enabled)
                    OnOpenAnyMenu();
                OpenInventory();
            }
        }
    }

    private void OnOpenAnyMenu()
    {
        _controlState = ControlState.Menu;
        _organism.Movement.Move(_organism, Vector2.zero, false);
    }

    private void OnCloseAnyMenu()
    {
        _controlState = ControlState.World;
    }

    private void OpenInventory()
    {
        _inventoryUI.Enabled = true;
    }

    private void CloseInventory()
    {
        _inventoryUI.Enabled = false;
    }

    private void ParseAndUpdateMenuClose()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_inventoryUI.Enabled)
            {
                CloseInventory();
            }
            if (_scavengingUI.enabled)
            {
                CloseContainer();
            }

            OnCloseAnyMenu();
        }
    }

    private void ParseAndUpdateItemInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            InventoryItem currentItem = _scavengingUI.CurrentItem;
            if (currentItem != null)
            {
                Inventory inventory = _organism.Inventory;
                if (inventory.CanTakeItem(currentItem))
                    _openContainer.TakeItem(currentItem, inventory.Container);
            }
        }
    }
}
