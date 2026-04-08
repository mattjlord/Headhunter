using UnityEngine;

[System.Serializable]
public class BehaviorTask
{
    private AIOrganism _organism;

    [SerializeField] private float _priority;
    private bool _isFrozen;
    private bool _isRunning;
    [SerializeField] private bool _isEssential = false;

    [SerializeField] private string _debugName;
    [SerializeField] protected string description;

    public string Description { get { return description; } }

    public BehaviorTask(AIOrganism organism)
    {
        _organism = organism;
        _isRunning = false;
    }

    public float Priority
    {
        get { return _priority; }
        set { _priority = value; }
    }

    public bool IsFrozen
    {
        get { return _isFrozen; }
        set { _isFrozen = value; }
    }

    public bool IsEssential
    {
        get { return _isEssential; }
        set { _isEssential = value;}
    }

    public bool IsRunning { get => _isRunning; }

    public void UpdateDebugInfo()
    {
        _debugName = GetName();
    }

    public void Run()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            Start();
        }
        Update();

        // TODO: Remove this later
        _organism.TaskMsg = GetName();
    }

    public void Exit()
    {
        if (_isRunning)
        {
            _isRunning = false;
            _organism.Navigation.StopMovement(_organism);
            Stop();
        }
    }

    public AIOrganism Organism { get {  return _organism; } }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Stop() { }
    public virtual void UpdatePriority() { }
    public virtual bool HasAssociatedLocation(ALocation location) { return false; }
    public virtual string GetName()
    {
        return "No Task";
    }
}