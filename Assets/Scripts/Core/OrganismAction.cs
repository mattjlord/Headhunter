using System;
using UnityEngine;

[Serializable]
public class OrganismAction
{
    private bool _constructed = false;
    private Organism _organism;
    [SerializeField] private string? _animationName;
    [SerializeField] [Range(0,1)] private float _progress;
    private float _duration = 0;
    private Action _triggeredAction;
    [SerializeField] [Range(0, 1)] private float _triggerDelay = 0;
    private Vector2 _displacement;
    [SerializeField] [Range(0, 1)] private float _displacementDelay = 0;

    private float _elapsedTime;
    [SerializeField] private bool _displacementStarted;
    [SerializeField] private bool _triggered;
    private Vector2 _startPos;
    private Vector2 _endPos;

    public OrganismAction(Organism organism)
    {
        _organism = organism;
        _constructed = true;
    }

    public bool Constructed { get { return _constructed; } }

    public Organism Organism { get { return _organism; } }

    public string? AnimationName
    {
        get { return _animationName; }
        set { _animationName = value; }
    }

    public float Duration
    {
        get { return _duration; }
        set { _duration = value; }
    }

    public Action TriggeredAction
    {
        get { return _triggeredAction; }
        set { _triggeredAction = value; }
    }

    public float TriggerDelay
    {
        get { return _triggerDelay; }
        set { _triggerDelay = Mathf.Clamp(value, 0, 1); }
    }

    public Vector2 Displacement
    {
        get { return _displacement; }
        set { _displacement = value; }
    }

    public float DisplacementDelay
    {
        get { return _displacementDelay; }
        set { _displacementDelay = Mathf.Clamp(value, 0, 1); }
    }

    public void Start(Animator animator)
    {
        if (_animationName != null)
        {
            animator.SetBool("Is Busy", true);
            animator.Play(_animationName, -1, 0);
        }

        _elapsedTime = 0;
        _displacementStarted = false;
        _triggered = false;

        _startPos = _organism.Position;
        _endPos = _organism.Position + _displacement;
    }

    public void Update(float deltaTime)
    {
        _organism.Movement.StopMovement();

        _elapsedTime += deltaTime;

        _progress = _elapsedTime / _duration;

        if (!_displacementStarted && _progress >= _displacementDelay)
            _displacementStarted = true;

        if (!_triggered && _progress >= _triggerDelay)
        {
            _triggeredAction?.Invoke();
        }

        if (_displacement != Vector2.zero && _displacementStarted)
        {
            Debug.DrawLine(VectorUtils.Vec2ToVec3(_startPos), VectorUtils.Vec2ToVec3(_endPos), Color.cyan);
            float displacementProgressBase = _progress - _displacementDelay;
            float displacementProgressMax = 1 - _displacementDelay;
            float displacementProgress = displacementProgressBase / displacementProgressMax;

            _organism.Position = Vector2.Lerp(_startPos, _endPos, displacementProgress);
        }
    }

    public bool IsFinished
    {
        get => _progress >= 1f;
    }

    public float Progress { get { return _progress; } }
}