using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed; // TODO: Integrate vitals into speed
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _footstepFrequency;
    [SerializeField] private float _footstepLoudness;
    private float _lastFootstep;
    private Vector2 _dir;
    private Organism _organism;
    private bool _isRunning = false;

    private void Start()
    {
        _dir = Vector2.zero;
    }

    public bool IsMoving { get { return _dir != Vector2.zero; } }

    public Vector2 Velocity
    {
        get 
        {
            float speed;

            if (_isRunning)
                speed = _runSpeed;
            else
                speed = _walkSpeed;

            return _dir * speed;
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

        footstep.Location = pointLocation;
        footstep.SenseType = SenseType.Sound;
        footstep.DetectableDistance = _footstepLoudness;
        footstep.ProducerOrganism = _organism;

        Instantiate(footstepObj, VectorUtils.Vec2ToVec3(_organism.Position), Quaternion.identity);

        footstep.Fire();

        // TODO: Footprints for the player to see (other organisms are too dumb to recognize them, no need to generate stimulus)
    }
}