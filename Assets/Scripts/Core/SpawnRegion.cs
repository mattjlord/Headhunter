using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegion : MonoBehaviour
{
    [SerializeField] private Organism _player;
    [SerializeField] private MasterOrganismManager _masterOrganismManager;
    [SerializeField] private int _density;
    [SerializeField] private float _tickRate = 5f;
    [SerializeField] private List<OrganismType> _organismTypes;

    private AreaLocation _areaLocation;
    private AreaLocation[] _subregions;

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

    public void SpawnAllOrganisms()
    {
        Dictionary<AreaLocation, Dictionary<OrganismType, List<AIOrganism>>> herds = new Dictionary<AreaLocation, Dictionary<OrganismType, List<AIOrganism>>>();

        for (int i = _activeOrganisms.Count; i < _density; i++)
        {
            int subregionIdx = Random.Range(0, _subregions.Length - 1);
            AreaLocation subregion = _subregions[subregionIdx];

            Vector2 spawnPoint = subregion.GetRandomPointInArea();

            int organismTypeIdx = Random.Range(0, _organismTypes.Count - 1);
            OrganismType organismType = _organismTypes[organismTypeIdx];

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

    private void CheckForPlayer()
    {
        float distanceToPlayer = _areaLocation.GetDistanceFrom(_player.Position);
        if (distanceToPlayer < 200f)
            SpawnAllOrganisms();
    }

    private AIOrganism SpawnOrganism(OrganismType organismType, Vector2 spawnPoint)
    {
        return _masterOrganismManager.SpawnOrganism(organismType, spawnPoint);
    }

    private void ConfigureHerd(List<AIOrganism> herd)
    {
        foreach (AIOrganism organism in herd)
            organism.HerdManagement.Herd = herd;
    }
}
