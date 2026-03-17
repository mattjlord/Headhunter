using UnityEngine;

public abstract class AMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed; // TODO: Integrate vitals into speed
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _footstepFrequency;
    [SerializeField] private float _footstepLoudness;

    [SerializeField] private GameObject _footprintPrefab;

    protected Vector2 dir;
    protected Organism organism;

    private float _lastFootstep;
    private bool _isRunning = false;

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
            if (_isRunning)
                return _runSpeed;
            else
                return _walkSpeed;
        }
    }

    private void Update()
    {
        if (organism == null)
            return;
        if (dir != Vector2.zero)
        {
            organism.LookDirection = dir;
            if (Time.time > _lastFootstep + _footstepFrequency)
                Footstep();
        }
        float speed;
        if (_isRunning)
            speed = _runSpeed;
        else
            speed = _walkSpeed;

        UpdateMove(speed);
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
    }

    protected abstract void Move(Vector2 value); // Value is either a direction (PlayerMovement) or a destination (AIMovement)
    protected abstract void UpdateMove(float speed);
    public abstract void StopMovement();

    private void Footstep()
    {
        _lastFootstep = Time.time;

        GameObject footstepObj = new GameObject();
        footstepObj.AddComponent<Stim_Footstep>();
        footstepObj.AddComponent<PointLocation>();

        Stim_Footstep footstep = footstepObj.GetComponent<Stim_Footstep>();
        footstep.OrganismType = organism.OrganismType;
        PointLocation pointLocation = footstepObj.GetComponent<PointLocation>();

        float loudness = _footstepLoudness;
        if (_isRunning) { loudness *= 2f; }

        footstep.Location = pointLocation;
        footstep.SenseType = SenseType.Sound;
        footstep.DetectableDistance = loudness;
        footstep.ProducerOrganism = organism;

        Vector3 footstepPos = VectorUtils.Vec2ToVec3(organism.Position);

        GameObject instance = Instantiate(footstepObj, footstepPos, Quaternion.identity);

        instance.GetComponent<Stim_Footstep>().Fire();

        if (_footprintPrefab != null)
        {
            Vector3 footprintPos = VectorUtils.Vec2ToVec3(organism.Position);
            Vector2 lookDir = organism.LookDirection;

            float angleInRadians = Mathf.Atan2(lookDir.x, lookDir.y);
            float angleInDeg = Mathf.Rad2Deg * angleInRadians;

            footprintPos.y = 0.01f;
            GameObject footstepInstance = Instantiate(_footprintPrefab, footprintPos, Quaternion.Euler(0, angleInDeg, 0));
            Destroy(footstepInstance, 1f);

        }
    }
}