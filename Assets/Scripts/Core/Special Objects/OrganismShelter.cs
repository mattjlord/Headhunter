using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismShelter : WorldObject
{
    [SerializeField] private Transform _shelterPoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private float _capacity = 4;
    [SerializeField] private float _exhaustionRecoveryRate;
    [SerializeField] private float _injuryRecoveryRate;
    [SerializeField] private float _heatRecoveryRate;

    [SerializeField] private List<Organism> _organismsInShelter = new List<Organism>();

    public bool AtCapacity => _organismsInShelter.Count >= _capacity;

    private void Update()
    {
        float exhaustionRecovery = _exhaustionRecoveryRate * TimeManagement.GameDeltaTime;
        float injuryRecovery = _injuryRecoveryRate * TimeManagement.GameDeltaTime;
        float heatRecovery = _heatRecoveryRate * TimeManagement.GameDeltaTime;

        foreach (Organism organism in _organismsInShelter)
        {
            organism.transform.position = _shelterPoint.position;
            organism.Vitals.GetVital(VitalType.Exhaustion).DecreaseValue(exhaustionRecovery);
            organism.Vitals.GetVital(VitalType.Injury).DecreaseValue(injuryRecovery);
            organism.Vitals.GetVital(VitalType.Heat).DecreaseValue(heatRecovery);
        }
    }

    public void Enter(Organism organism)
    {
        if (_organismsInShelter.Contains(organism)) return;

        Debug.Log("Entering shelter!");

        _organismsInShelter.Add(organism);
        organism.Movement.HideFromNavMesh();
        organism.transform.position = _shelterPoint.position;
    }

    public void Exit(Organism organism)
    {
        Debug.Log("Calling 'exit' on this shelter");

        if (!_organismsInShelter.Contains(organism))
            return;

        Debug.Log("Exit successful!");

        organism.transform.position = _exitPoint.position;
        _organismsInShelter.Remove(organism);
    }
}
