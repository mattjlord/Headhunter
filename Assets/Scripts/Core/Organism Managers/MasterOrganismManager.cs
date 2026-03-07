using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MasterOrganismManager : MonoBehaviour
{
    [SerializeField] private OrganismManager _mudyakManager;
    [SerializeField] private OrganismManager _bulletRaptorManager;
    [SerializeField] private int _maxOrganisms = 100;
    [SerializeField] private float _despawnDistance = 400f;
    [SerializeField] private PlayerOrganism _player;

    public float DespawnDistance { get => _despawnDistance; }
    public PlayerOrganism Player { get => _player; }

    private List<AIOrganism> _allOrganisms;

    public bool CanSpawnOrganism(OrganismType organismType)
    {
        if (_allOrganisms.Count == _maxOrganisms) { return false; }
        return GetOrganismManager(organismType).CanSpawn();
    }

    private void Start()
    {
        _allOrganisms = new List<AIOrganism>();
    }

    public AIOrganism SpawnOrganism(OrganismType organismType, Vector2 point)
    {
        AIOrganism organism = InstantiateOrganism(organismType, point);
        _allOrganisms.Add(organism);

        return organism;
    }

    public void RemoveOrganism(AIOrganism organism)
    {
        _allOrganisms.Remove(organism);
        OrganismManager manager = GetOrganismManager(organism.OrganismType);
        manager.RemoveOrganism(organism);
    }

    private AIOrganism InstantiateOrganism(OrganismType organismType, Vector2 point)
    {
        return GetOrganismManager(organismType).SpawnOrganism(point);
    }

    private OrganismManager GetOrganismManager(OrganismType organismType)
    {
        switch (organismType)
        {
            case OrganismType.Mudyak:
                return _mudyakManager;
            case OrganismType.BulletRaptor:
                return _bulletRaptorManager;
            default:
                return null;
        }
    }
}
