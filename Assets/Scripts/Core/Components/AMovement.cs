using UnityEngine;

public abstract class AMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _footstepFrequency;
    [SerializeField] private float _footstepLoudness;

    [SerializeField] private GameObject _footprintPrefab;

    [SerializeField] private Animator _animator;

    private float _currentSpeed;

    private float _encumbranceModifier = 1f;
    private float _exhaustionModifier = 1f;

    protected Vector2 dir;
    protected Organism organism;

    private float _lastFootstep;
    private bool _isRunning = false;

    public float EncumbranceModifier { set { _encumbranceModifier = value; } }
    public float ExhaustionModifier { set { _exhaustionModifier = value; } }

    protected virtual void Awake()
    {
        dir = Vector2.zero;
    }

    public bool IsMoving { get { return dir != Vector2.zero; } }

    public Vector2 Velocity
    {
        get { return dir * CurrentSpeed; }
    }

    public float CurrentSpeed
    {
        get
        {
            if (dir == Vector2.zero)
                return 0f;
            else
                return _currentSpeed;
        }
    }

    private void Update()
    {
        if (organism == null)
            return;
        if (dir != Vector2.zero)
        {
            organism.LookDirection = dir;
            if (_footstepLoudness > 0 && Time.time > _lastFootstep + _footstepFrequency)
                Footstep();
        }
        if (_isRunning)
            _currentSpeed = _runSpeed;
        else
            _currentSpeed = _walkSpeed;

        _currentSpeed *= _encumbranceModifier;
        _currentSpeed *= _exhaustionModifier;

        UpdateMove(_currentSpeed);
        if (_animator != null)
            UpdateAnimation();
    }

    public void Move(Organism organism, Vector2 value, bool run)
    {
        AssignMoveParams(organism, run);
        Move(value);
    }

    private void AssignMoveParams(Organism organism, bool run)
    {
        this.organism = organism;
        _isRunning = run;

        // TODO: Update vitals with extra impact if the organism is walking/running
    }

    protected abstract void Move(Vector2 value); // Value is either a direction (PlayerMovement) or a destination (AIMovement)
    protected abstract void UpdateMove(float speed);
    public abstract void StopMovement();

    private void Footstep()
    {
        _lastFootstep = Time.time;

        if (_footprintPrefab != null)
        {
            Vector3 footprintPos = VectorUtils.Vec2ToVec3(organism.Position);
            Vector2 lookDir = organism.LookDirection;

            float angleInRadians = Mathf.Atan2(lookDir.x, lookDir.y);
            float angleInDeg = Mathf.Rad2Deg * angleInRadians;

            footprintPos.y = 0.01f;
            GameObject footstepInstance = Instantiate(_footprintPrefab, footprintPos, Quaternion.Euler(0, angleInDeg, 0));

            Stim_Footstep stim = footstepInstance.GetComponent<Stim_Footstep>();
            stim.DetectableDistance = _footstepLoudness;
            stim.Fire();

            Destroy(footstepInstance, 1f);

        }
    }

    private void UpdateAnimation()
    {
        int moveState;
        // TODO: Remove later
        string debugMsg;

        if (CurrentSpeed == 0)
        {
            moveState = 0;
            debugMsg = "Stationary";
        }
        else if (!_isRunning)
        {
            moveState = 1;
            debugMsg = "Walking";
        }
        else
        {
            moveState = 2;
            debugMsg = "Running";
        }

        _animator.SetInteger("Move State", moveState);
        _animator.SetFloat("Speed", _currentSpeed);

        AIOrganism aiOrganism = organism as AIOrganism;
        if (aiOrganism != null)
        {
            aiOrganism.MoveMsg = debugMsg;
        }
    }

    public abstract void HideFromNavMesh();
}