using System;

public class OrganismAction
{
    private string? _animationName;
    private float _duration = 0;
    private Action _triggeredAction;
    private float _triggerDelay = 0;

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
        set { _triggerDelay = value; }
    }
}