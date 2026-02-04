public class BehaviorTask
{
    private AIOrganism _organism;

    private float _priority;
    private bool _isFrozen;
    private bool _isRunning;
    private bool _isEssential = false;

    protected string description;

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

    public void Run()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            Start();
        }
        Update();
    }

    public void Exit()
    {
        if (_isRunning)
        {
            _isRunning = false;
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