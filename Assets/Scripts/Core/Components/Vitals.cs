using UnityEngine;

public class Vitals : MonoBehaviour
{
    [SerializeField] private Vital _hunger;
    [SerializeField] private Vital _thirst;
    [SerializeField] private Vital _exhaustion;
    [SerializeField] private Vital _heat;
    [SerializeField] private Vital _injury;
    [SerializeField] private Vital _toxicity;

    private bool _awake = true;
    private bool _alive = true;

    public void Update()
    {
        _hunger.Update();
        _thirst.Update();
        _exhaustion.Update();
        _heat.Update();
        _injury.Update();
        _toxicity.Update();
    }

    public Vital GetVital(VitalType vital)
    {
        switch (vital)
        {
            case VitalType.Hunger: return _hunger;
            case VitalType.Thirst: return _thirst;
            case VitalType.Exhaustion: return _exhaustion;
            case VitalType.Heat: return _heat;
            case VitalType.Injury: return _injury;
            default: return _toxicity;
        }
    }
}
