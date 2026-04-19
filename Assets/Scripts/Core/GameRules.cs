using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    [SerializeField] private PlayerOrganism _player;
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private GameOverUI _gameOverUI;

    [SerializeField] private List<OrganismType> _organismsKilled;

    private void Start()
    {
        _organismsKilled = new List<OrganismType>();
        _player.OnOrganismKilled += LogOrganismKill;

        InitDeathEvents();
    }

    public void InitDeathEvents()
    {
        InitDeathEvent(VitalType.Hunger);
        InitDeathEvent(VitalType.Thirst);
        InitDeathEvent(VitalType.Exhaustion);
        InitDeathEvent(VitalType.Heat);
        InitDeathEvent(VitalType.Injury);
        InitDeathEvent(VitalType.Toxicity);
    }

    private void InitDeathEvent(VitalType type)
    {
        _player.Vitals.GetVital(type).OnMaxValueReached += () => Die(type);
    }

    private void LogOrganismKill(Organism organism)
    {
        _organismsKilled.Add(organism.OrganismType);
    }

    private void Die(VitalType type)
    {
        _playerController.ControlsEnabled = false;
        _player.Movement.StopMovement();
        //_player.ActionManagement.Stop();
        _gameOverUI.DisplayGameOverUI(type, _organismsKilled);
    }
}
