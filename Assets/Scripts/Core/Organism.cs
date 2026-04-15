using System;
using UnityEngine;

public class Organism : WorldObject
{
    public event Action<Organism> OnDie;
    public event Action<Stimulus> OnStimulus;

    [SerializeField] private Vitals _vitals;
    [SerializeField] private Senses _senses;
    [SerializeField] private AMovement _movement;
    [SerializeField] private Odor _odor;
    [SerializeField] private Visibility _visibility;
    [SerializeField] private ActionManagement _actionManagement;
    [SerializeField] private MeleeAttack _meleeAttack;
    [SerializeField] private OrganismType _organismType;
    [SerializeField] private float _reach;
    [SerializeField] private float _combatReach;

    [SerializeField] private Color _debugColor;

    [SerializeField] private GameObject _bloodPrefab;

    private Vector2 _lookDirection = Vector2.up;

    public Vitals Vitals { get { return _vitals; } }
    public Senses Senses { get { return _senses; } }
    public AMovement Movement { get { return _movement; } }
    public Odor Odor { get { return _odor; } }
    public Visibility Visibility { get { return _visibility; } }
    public ActionManagement ActionManagement { get { return _actionManagement; } }
    public MeleeAttack MeleeAttack { get { return _meleeAttack; } }
    public OrganismType OrganismType { get { return _organismType; } }
    public float Reach { get { return _reach; } }
    public float CombatReach {  get { return _combatReach; } }

    protected virtual void Start()
    {
        _odor.Organism = this;
        _visibility.Organism = this;

        // Vital impacts - add more as needed
        _vitals.GetVital(VitalType.Exhaustion).OnValueChanged += OnExhaustionChanged;
    }

    public Vector2 LookDirection
    {
        get { return _lookDirection; }
        set
        {
            _lookDirection = value;
            float angleInRadians = Mathf.Atan2(value.x, value.y);
            float angleInDeg = Mathf.Rad2Deg * angleInRadians;
            transform.rotation = Quaternion.Euler(0, angleInDeg, 0);
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = _debugColor;
        Vector3 pos = VectorUtils.Vec2ToVec3(Position);
        pos.y = 0.25f;
        Gizmos.DrawWireSphere(pos, _reach);
        Gizmos.DrawWireSphere(pos, _combatReach);

        Gizmos.color = Color.white;
        Vector3 worldLookDir = VectorUtils.Vec2ToVec3(_lookDirection);
        Gizmos.DrawRay(pos, worldLookDir * 5f);
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

    public void OnOrganismDie()
    {
        OnDie?.Invoke(this);
        //TODO: Actual player death logic later, this is a band-aid!
        if (GetType() == typeof(PlayerOrganism))
        {
            Application.Quit();
        }
    }

    public void RespondToStimulus(Stimulus stimulus)
    {
        if (Senses.CanSense(stimulus))
            OnStimulus?.Invoke(stimulus);
    }

    private void OnExhaustionChanged(float value)
    {
        float movementModifier = Mathf.Lerp(1, 0.5f, value / 100f);
        _movement.ExhaustionModifier = movementModifier;
    }

    public void TakeDamage(float damage, DamageType damageType, Vector2? worldSource, Vector3? impactPoint)
    {
        VitalType vitalType = VitalType.Injury;

        switch (damageType)
        {
            case DamageType.Physical:
                vitalType = VitalType.Injury;
                if (impactPoint != null && _bloodPrefab != null)
                    Instantiate(_bloodPrefab, (Vector3)impactPoint, Quaternion.identity);
                break;
            case DamageType.Toxic:
                vitalType = VitalType.Toxicity;
                break;
            case DamageType.Fire:
                vitalType = VitalType.Heat;
                break;
        }

        _vitals.GetVital(vitalType).IncreaseValue(damage);

        if (worldSource != null)
        {
            Vector2 lookDir = ((Vector2)worldSource - Position).normalized;
            LookDirection = lookDir;
        }
    }
}
