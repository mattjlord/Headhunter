using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed; // TODO: Integrate vitals into speed
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _footstepFrequency;
    [SerializeField] private float _footstepLoudness;

    [SerializeField] private GameObject _footprintPrefab;

    private float _lastFootstep;
    private Vector2 _dir;
    private Organism _organism;
    private bool _isRunning = false;

    private void Awake()
    {
        _dir = Vector2.zero;
    }

    public bool IsMoving { get { return _dir != Vector2.zero; } }

    public Vector2 Velocity
    {
        get { return _dir * CurrentSpeed; }
    }

    public float CurrentSpeed
    {
        get
        {
            if (_dir == Vector2.zero)
                return 0f;
            if (_isRunning)
                return _runSpeed;
            else
                return _walkSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (_organism == null)
            return;
        if (_dir != Vector2.zero)
        {
            _organism.LookDirection = _dir;
            if (Time.fixedTime > _lastFootstep + _footstepFrequency)
                Footstep();
        }
        float speed;
        if (_isRunning)
            speed = _runSpeed;
        else
            speed = _walkSpeed;


        _organism.Position += (_dir * speed);
    }

    public void Move(Organism organism, Vector2 dir, bool run)
    {
        _organism = organism;
        _dir = dir;
        _isRunning = run;
    }

    private void Footstep()
    {
        _lastFootstep = Time.fixedTime;

        GameObject footstepObj = new GameObject();
        footstepObj.AddComponent<Stim_Footstep>();
        footstepObj.AddComponent<PointLocation>();

        Stim_Footstep footstep = footstepObj.GetComponent<Stim_Footstep>();
        PointLocation pointLocation = footstepObj.GetComponent<PointLocation>();

        float loudness = _footstepLoudness;
        if (_isRunning) { loudness *= 2f; }

        footstep.Location = pointLocation;
        footstep.SenseType = SenseType.Sound;
        footstep.DetectableDistance = loudness;
        footstep.ProducerOrganism = _organism;

        Vector3 footstepPos = VectorUtils.Vec2ToVec3(_organism.Position);

        Instantiate(footstepObj, footstepPos, Quaternion.identity);

        footstep.Fire();

        Debug.DrawRay(footstepPos + Vector3.up, loudness * Vector3.right, Color.red, 1f);
        Debug.DrawRay(footstepPos + Vector3.up, loudness * Vector3.back, Color.red, 1f);
        Debug.DrawRay(footstepPos + Vector3.up, loudness * Vector3.left, Color.red, 1f);
        Debug.DrawRay(footstepPos + Vector3.up, loudness * Vector3.forward, Color.red, 1f);

        if (_footprintPrefab != null)
        {
            /*Vector3 footprintPos = VectorUtils.Vec2ToVec3(_organism.Position);
            Vector2 lookDir = _organism.LookDirection;

            float angleInRadians = Mathf.Atan2(lookDir.x, lookDir.y);
            float angleInDeg = Mathf.Rad2Deg * angleInRadians;

            footprintPos.y = 0.01f;
            Instantiate(_footprintPrefab, footprintPos, Quaternion.Euler(0, angleInDeg, 0));*/

        }
    }
}