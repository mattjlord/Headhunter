using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiObject;
    [SerializeField] private TMP_Text _gameOverDetailsMsg;

    private bool _isGameOver = false;

    public void DisplayGameOverUI(VitalType vitalType, List<OrganismType> organismsKilled)
    {
        if (_isGameOver)
            return;

        _isGameOver = true;

        _uiObject.SetActive(true);

        string causeOfDeath = "";

        switch (vitalType)
        {
            case VitalType.Hunger:
                causeOfDeath = "You starved to death.";
                break;
            case VitalType.Thirst:
                causeOfDeath = "You died of thirst.";
                break;
            case VitalType.Exhaustion:
                causeOfDeath = "You collapsed from exhaustion.";
                break;
            case VitalType.Heat:
                causeOfDeath = "You overheated.";
                break;
            case VitalType.Injury:
                causeOfDeath = "You died from your wounds.";
                break;
            case VitalType.Toxicity:
                causeOfDeath = "You died from a mysterious illness.";
                break;
        }

        int days = TimeManagement.Day;
        int hours = TimeManagement.Hours - TimeManagement.StartHour;
        int minutes = TimeManagement.Minutes;

        string timeOfDeath = $"You survived for {days} days, {hours} hours, and {minutes} minutes.";

        string killCount = $"You hunted and killed {organismsKilled.Count} organisms.";

        string combinedMessage = causeOfDeath + "\r\n" + timeOfDeath + "\r\n" + killCount;

        _gameOverDetailsMsg.text = combinedMessage;
    }
}
