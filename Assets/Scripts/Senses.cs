using UnityEngine;

public class Senses : MonoBehaviour
{
    [SerializeField] private Transform _headTransform;
    [SerializeField] private float _sightRadius;
    [SerializeField] private float _fov;
    [SerializeField] private float _hearingRadius;
    [SerializeField] private float _smellRadius;
    public bool CanSense(AStimulus stimulus)
    {
        switch (stimulus.SenseType)
        {
            case SenseType.Sight:
                return CanSee(stimulus);
            case SenseType.Sound:
                return CanHear(stimulus);
            default:
                return CanSmell(stimulus);
        }
    }

    private bool CanSee(AStimulus stimulus)
    {
        // TODO
        return true;
    }

    private bool CanHear(AStimulus stimulus)
    {
        // TODO
        return true;
    }

    private bool CanSmell(AStimulus stimulus)
    {
        // TODO
        return true;
    }
}
