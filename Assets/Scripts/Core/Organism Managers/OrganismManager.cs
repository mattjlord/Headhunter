using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismManager : MonoBehaviour
{
    [SerializeField] private int _maxOrganisms = 30;

    [SerializeField] private GameObject _spawnTemplate;

    [SerializeField, Range(0f, 100f)] float _minHunger = 0f;
    [SerializeField, Range(0f, 100f)] float _maxHunger = 100f;

    [SerializeField, Range(0f, 100f)] float _minThirst = 0f;
    [SerializeField, Range(0f, 100f)] float _maxThirst = 100f;

    [SerializeField, Range(0f, 100f)] float _minExhaustion = 0f;
    [SerializeField, Range(0f, 100f)] float _maxExhaustion = 100f;

    [SerializeField, Range(0f, 100f)] float _minHeat = 0f;
    [SerializeField, Range(0f, 100f)] float _maxHeat = 100f;

    [SerializeField, Range(0f, 100f)] float _minInjury = 0f;
    [SerializeField, Range(0f, 100f)] float _maxInjury = 100f;

    [SerializeField] private List<ALocation> _foodLocations;
    [SerializeField] private List<ALocation> _waterLocations;
    [SerializeField] private List<ALocation> _shelterLocations;

    private List<AIOrganism> _organisms;

    private void Start()
    {
        _organisms = new List<AIOrganism>();
    }

    public bool CanSpawn() => _organisms.Count < _maxOrganisms;

    public AIOrganism SpawnOrganism(Vector2 point)
    {
        Vector3 worldPoint = VectorUtils.Vec2ToVec3(point);
        GameObject spawnedInstance = Instantiate(_spawnTemplate, worldPoint, Quaternion.identity);
        AIOrganism organism = _spawnTemplate.GetComponent<AIOrganism>();

        AssignVital(organism, VitalType.Hunger, _minHunger, _maxHunger);
        AssignVital(organism, VitalType.Thirst, _minThirst, _maxThirst);
        AssignVital(organism, VitalType.Exhaustion, _minExhaustion, _maxExhaustion);
        AssignVital(organism, VitalType.Heat, _minHeat, _maxHeat);
        AssignVital(organism, VitalType.Injury, _minInjury, _maxInjury);

        organism.LocationKnowledge.FoodLocations = _foodLocations;
        organism.LocationKnowledge.WaterLocations = _waterLocations;
        organism.LocationKnowledge.ShelterLocation = _shelterLocations;

        _organisms.Add(organism);

        return organism;
    }

    private void AssignVital(Organism organism, VitalType vitalType, float min, float max)
    {
        float value = Random.Range(min, max);
        organism.Vitals.GetVital(vitalType).Value = value;
    }
}
