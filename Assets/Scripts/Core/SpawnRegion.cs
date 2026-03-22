using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRegion : MonoBehaviour
{
    [SerializeField] private MasterOrganismManager _masterOrganismManager;
    [SerializeField] private int _organismDensity;
    [SerializeField] private int _plantDensity;
    [SerializeField] private float _tickRate = 5f;
    [SerializeField] private List<OrganismType> _organismTypes;

    // Plant spawning can be more complex in the future if I want
    [SerializeField] private List<GameObject> _plantPrefabs;
    private int _plantCount = 0;

    private AreaLocation _areaLocation;
    [SerializeField] private AreaLocation[] _subregions;

    private List<AIOrganism> _activeOrganisms;

    private float _timeSinceLastTick = 0f;

    private void Start()
    {
        _areaLocation = GetComponent<AreaLocation>();
        _subregions = GetComponentsInChildren<AreaLocation>();

        _activeOrganisms = new List<AIOrganism>();
    }

    private void Update()
    {
        _timeSinceLastTick += Time.deltaTime;

        if (_timeSinceLastTick > _tickRate)
        {
            CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        float distanceToPlayer = _areaLocation.GetDistanceFrom(_masterOrganismManager.Player.Position, OrganismType.Hunter);
        if (distanceToPlayer < 200f)
        {
            SpawnAllPlants();
            SpawnAllOrganisms();
        }
    }

    public void SpawnAllPlants()
    {
        int i = _plantCount;

        while (i < _plantDensity)
        {
            int subregionIdx = Random.Range(0, _subregions.Length);
            AreaLocation subregion = _subregions[subregionIdx];

            Vector2 spawnPoint = subregion.GetRandomPointInArea();

            if (!NavUtils.PointInBounds(OrganismType.Mudyak, spawnPoint, 1f, out NavMeshHit hit)) // This is kinda messy, using mudyak NavMesh for validation
            {
                continue;
            }

            int plantPrefabIdx = Random.Range(0, _plantPrefabs.Count);
            GameObject plantPrefab = _plantPrefabs[plantPrefabIdx];

            Vector3 worldSpawnPoint = VectorUtils.Vec2ToVec3(spawnPoint);

            float rotation = Random.Range(0, 359);

            Instantiate(plantPrefab, worldSpawnPoint, Quaternion.AngleAxis(rotation, Vector3.up));

            _plantCount++;
            i++;
        }
    }

    public void SpawnAllOrganisms()
    {
        Dictionary<AreaLocation, Dictionary<OrganismType, List<AIOrganism>>> herds = new Dictionary<AreaLocation, Dictionary<OrganismType, List<AIOrganism>>>();

        int i = _activeOrganisms.Count;

        while (i < _organismDensity)
        {
            int subregionIdx = Random.Range(0, _subregions.Length);
            AreaLocation subregion = _subregions[subregionIdx];

            Vector2 spawnPoint = subregion.GetRandomPointInArea();

            int organismTypeIdx = Random.Range(0, _organismTypes.Count);
            OrganismType organismType = _organismTypes[organismTypeIdx];

            if (!NavUtils.PointInBounds(organismType, spawnPoint, 1f, out NavMeshHit hit))
            {
                continue;
            }

            if (!_masterOrganismManager.CanSpawnOrganism(organismType))
            {
                return;
            }

            AIOrganism spawnedOrganism = SpawnOrganism(organismType, spawnPoint);

            if (!herds.ContainsKey(subregion)) { herds[subregion] = new Dictionary<OrganismType, List<AIOrganism>>(); }
            var subregionHerds = herds[subregion];
            if (!subregionHerds.ContainsKey(organismType)) { subregionHerds[organismType] = new List<AIOrganism>(); }
            List<AIOrganism> herd = subregionHerds[organismType];
            herd.Add(spawnedOrganism);

            _activeOrganisms.Add(spawnedOrganism);

            i++;
        }

        foreach (AreaLocation subregion in herds.Keys)
        {
            var subregionHerds = herds[subregion];
            foreach  (OrganismType organismType in subregionHerds.Keys)
            {
                List<AIOrganism> herd = subregionHerds[organismType];
                ConfigureHerd(herd);
            }
        }
    }

    private void RespondToOrganismPos(WorldObject obj)
    {
        if (obj.GetType() != typeof(AIOrganism)) return;

        Organism organism = obj as Organism;

        if (!organism.IsPositionValid()) // Band-aid fix for bad spawn logic
        {
            organism.Despawn();
            RemoveOrganism(organism);
        }

        float distance = Vector2.Distance(_masterOrganismManager.Player.Position, organism.Position);

        if (distance > _masterOrganismManager.DespawnDistance)
        {
            organism.Despawn();
            RemoveOrganism(organism);
        }
    }

    private void RemoveOrganism(Organism organism)
    {
        AIOrganism aiOrganism = (AIOrganism)organism;
        _activeOrganisms.Remove(aiOrganism);
        organism.OnDie -= RemoveOrganism;
        organism.OnPositionUpdate -= RespondToOrganismPos;

        _masterOrganismManager.RemoveOrganism(aiOrganism);
    }

    private AIOrganism SpawnOrganism(OrganismType organismType, Vector2 spawnPoint)
    {
        AIOrganism organism = _masterOrganismManager.SpawnOrganism(organismType, spawnPoint);
        organism.OnPositionUpdate += RespondToOrganismPos;
        organism.OnDie += RemoveOrganism;
        return organism;
    }

    private void ConfigureHerd(List<AIOrganism> herd)
    {
        foreach (AIOrganism organism in herd)
            organism.HerdManagement.Herd = herd;
    }
}
