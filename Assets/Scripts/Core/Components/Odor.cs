using UnityEngine;

public class Odor : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] private int _trailMaxLength;
    private static float _frequency = 1.0f;
    private float _lastEmission;

    private Organism _organism;
    private OrganismType _organismType;

    private GameObject _odorTrailObj;
    private Stimulus _odorTrailStim;
    private TrailLocation _odorTrail;

    [SerializeField] private int _trailLength = 0;

    public Organism Organism 
    { 
        set 
        {
            _organism = value;
            _organismType = value.OrganismType;
            GenerateOdorTrail();
        } 
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime > _lastEmission + _frequency)
        {
            Emit();
            Cleanup();
        }

        _odorTrailStim.Fire();
    }

    private void GenerateOdorTrail()
    {
        GameObject odorTrailObj = new GameObject();
        AttachOdorTrailStimulus(odorTrailObj);
        odorTrailObj.AddComponent<TrailLocation>();


        GameObject instance = Instantiate(odorTrailObj);
        _odorTrailObj = instance;
        _odorTrailStim = _odorTrailObj.GetComponent<Stimulus>();
        _odorTrail = _odorTrailObj.GetComponent<TrailLocation>();

        _odorTrailStim.Location = _odorTrail;
        _odorTrailStim.SenseType = SenseType.Smell;
        _odorTrailStim.DetectableDistance = _strength;
        _odorTrailStim.ProducerOrganism = _organism;
        _odorTrailStim.Lingering = true;

        _odorTrailObj.transform.parent = transform;
    }

    private void AttachOdorTrailStimulus(GameObject obj)
    {
        switch (_organismType)
        {
            case OrganismType.Hunter:
                obj.AddComponent<Stim_Hunter>();
                return;
            case OrganismType.Mudyak:
                obj.AddComponent<Stim_Mudyak>();
                return;
            case OrganismType.BulletRaptor:
                obj.AddComponent<Stim_BulletRaptor>();
                return;
        }
    }

    private void Emit()
    {
        _lastEmission = Time.fixedTime;
        _trailLength++;

        _odorTrail.AddPoint(_organism.Position);
    }

    private void Cleanup()
    {
        if (_trailLength > _trailMaxLength)
        {
            _odorTrail.Reduce();
            _trailLength--;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _strength);
    }
}
