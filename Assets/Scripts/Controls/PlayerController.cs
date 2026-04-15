using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] private CraftingUI _craftingUI;
    [SerializeField] private ControlHintsUI _controlHintsUI;
    [SerializeField] private MapUI _mapUI;
    [SerializeField] private LayerMask _interactionLayers;

    public ControlState ControlState =>
        MenusEnabled ? ControlState.Menu : ControlState.World;

    private Vector2 _lookDirection = Vector2.up;
    private Vector2 _lookPoint;

    private bool _isRunning = false;
    private Vector2 _moveDirection = Vector2.zero;

    private Container _openContainer = null;

    private void Start() // TODO: GET RID OF THIS ASAP THIS IS A BAND-AID FIX
    {
        _organism.Vitals.GetVital(VitalType.Injury).OnMaxValueReached += () => Application.Quit();
        _inventoryUI.Enabled = false;
        _scavengingUI.Enabled = false;
        _craftingUI.Enabled = false;
        _controlHintsUI.HideAll();
        _mapUI.Enabled = false;
    }

    public void RestInShelter(PlayerShelter shelter)
    {
        StartCoroutine(RestInShelterAsync(shelter));
    }

    private IEnumerator RestInShelterAsync(PlayerShelter shelter)
    {
        _organism.Vitals.InShelter = true;

        float minutesElapsed = TimeManagement.SkipToNextSafePeriod();
        float exhaustionRecovery = shelter.RecoveryPerMinute * minutesElapsed;
        _organism.Vitals.GetVital(VitalType.Exhaustion).DecreaseValue(exhaustionRecovery);
        _organism.Vitals.GetVital(VitalType.Heat).Value = 0;

        yield return new WaitForSeconds(1); // TODO: More complex wait logic (fade screen, etc.)

        _organism.Vitals.InShelter = false;
    }

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

        switch(ControlState)
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

        UpdateMenuControlHints();
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
        _controlHintsUI.RMBText = "";

        if (_organism.LeechOrganism != null)
        {
            _controlHintsUI.RMBText = "Catch Wasp";

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {

            }

            return;
        }

        Vector3 worldLookPoint = VectorUtils.Vec2ToVec3(_lookPoint);
        bool objectHit = Physics.Raycast(worldLookPoint + 20 * Vector3.up, Vector3.down * 20f, out RaycastHit hitInfo, _interactionLayers);

        if (!objectHit) { return; }

        WorldObject obj = hitInfo.collider.gameObject.GetComponent<WorldObject>();

        if (obj == null) { return; }

        if (!obj.WithinReach(_organism.Position, _organism.Reach)) { return; }

        _controlHintsUI.RMBText = obj.GetInteractionPhrase();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            obj.OnInteraction(this);
        }
    }

    private void ParseAndUpdateMenuControls()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_inventoryUI.Enabled)
            {
                CloseInventory();
            }
            else
            {
                if (!MenusEnabled)
                    OnOpenAnyMenu();
                OpenInventory();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_craftingUI.Enabled)
            {
                CloseCrafting();
            }
            else
            {
                if (!MenusEnabled)
                    OnOpenAnyMenu();
                OpenCrafting();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            _mapUI.Enabled = !_mapUI.Enabled;
        }
    }

    private bool MenusEnabled => _scavengingUI.Enabled || _inventoryUI.Enabled || _craftingUI.Enabled;

    private void OnOpenAnyMenu()
    {
        _organism.Movement.Move(_organism, Vector2.zero, false);
    }

    private void OpenInventory() => _inventoryUI.Enabled = true;
    private void CloseInventory() =>_inventoryUI.Enabled = false;
    private void OpenCrafting() => _craftingUI.Enabled = true;
    private void CloseCrafting() => _craftingUI.Enabled = false;

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
            if (_craftingUI.enabled)
            {
                CloseCrafting();
            }
        }
    }

    private void ParseAndUpdateItemInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_scavengingUI.Enabled)
            {
                InventoryItemInstance currentItem = _scavengingUI.CurrentItem;
                if (currentItem != null)
                {
                    Inventory inventory = _organism.Inventory;
                    if (inventory.CanReceiveItem(currentItem) && _openContainer.Items.Contains(currentItem))
                        _openContainer.TakeItem(currentItem, inventory.Container);
                }

                currentItem = _inventoryUI.CurrentItem;
                if (currentItem != null && _inventoryUI.Enabled && _openContainer.CanAddItem())
                {
                    Inventory inventory = _organism.Inventory;
                    inventory.Container.TakeItem(currentItem, _openContainer);
                }
            }
            else if (_craftingUI.Enabled)
            {
                Crafting crafting = _organism.Crafting;
                Inventory inventory = _organism.Inventory;

                InventoryItemInstance currentInputItem = _craftingUI.CurrentInputItem;
                InventoryItemInstance currentOutputItem = _craftingUI.CurrentOutputItem;

                if (currentInputItem != null)
                {
                    if (inventory.CanReceiveItem(currentInputItem))
                        crafting.Inputs.TakeItem(currentInputItem, inventory.Container);
                }
                else if (currentOutputItem != null)
                {
                    if (inventory.CanReceiveItem(currentOutputItem))
                        crafting.Outputs.TakeItem(currentOutputItem, inventory.Container);
                }
                else if (_inventoryUI.Enabled)
                {
                    InventoryItemInstance currentInventoryItem = _inventoryUI.CurrentItem;
                    if (currentInventoryItem == null) return;

                    if (crafting.Inputs.CanAddItem() && currentInventoryItem.Item is CraftingMaterial)
                        inventory.Container.TakeItem(currentInventoryItem, crafting.Inputs);
                }
            }
        }

        if (_inventoryUI.Enabled) 
        {
            InventoryItemInstance currentItem = _inventoryUI.CurrentItem;
            if (currentItem != null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Inventory inventory = _organism.Inventory;
                    ItemInteractionType selectedOption = currentItem.InteractionType;
                    inventory.ProcessItemInteraction(currentItem, selectedOption, _organism);
                }

                float scroll = Input.GetAxis("Mouse ScrollWheel");

                if (scroll > 0)
                    currentItem.InteractionIndex++;
                else if (scroll < 0)
                    currentItem.InteractionIndex--;
            }
                
        }
    }

    private void UpdateMenuControlHints()
    {
        InventoryItemInstance inventoryItem = _inventoryUI.CurrentItem;
        InventoryItemInstance scavengingItem = _scavengingUI.CurrentItem;
        InventoryItemInstance craftingInputItem = _craftingUI.CurrentInputItem;
        InventoryItemInstance craftingOutputItem = _craftingUI.CurrentOutputItem;

        string moveDestName = "";

        if (inventoryItem != null)
        {
            // LMB
            if (_craftingUI.Enabled && inventoryItem.Item is CraftingMaterial)
                moveDestName = "Crafting Menu";
            else if (_scavengingUI.Enabled)
                moveDestName = _scavengingUI.Container.Name;

            // MWheel
            _controlHintsUI.MWheelText = "Switch Interaction";

            // RMB
            string interactionPhrase = LanguageUtils.GetInteractionPhrase(inventoryItem, inventoryItem.InteractionType);
            _controlHintsUI.RMBText = interactionPhrase + " Item";
        }
        else if (scavengingItem != null)
        {
            // LMB
            if (_inventoryUI.Enabled)
                moveDestName = "Inventory";

            // MWheel & RMB do nothing
            _controlHintsUI.MWheelText = "";
            _controlHintsUI.RMBText = "";
        }
        else if (craftingInputItem != null)
        {
            // LMB
            if (_inventoryUI.Enabled)
                moveDestName = "Inventory";

            // MWheel & RMB do nothing
            _controlHintsUI.MWheelText = "";
            _controlHintsUI.RMBText = "";
        }
        else if (craftingOutputItem != null)
        {
            // LMB
            if (_inventoryUI.Enabled)
                moveDestName = "Inventory";

            // MWheel & RMB do nothing
            _controlHintsUI.MWheelText = "";
            _controlHintsUI.RMBText = "";
        }
        else
        {
            _controlHintsUI.HideAll();
            return;
        }

        // Handle LMB move text here, in a final step
        if (moveDestName != "")
            _controlHintsUI.LMBText = "Move to " + moveDestName;
        else
            _controlHintsUI.LMBText = "";
    }
}
