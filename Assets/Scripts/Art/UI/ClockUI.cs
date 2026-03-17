using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _dayText;

    private void Update()
    {
        string hourText = TimeManagement.Hours.ToString("D2");
        string minText = TimeManagement.Minutes.ToString("D2");
        _timeText.text = hourText + ":" + minText;

        _dayText.text = "Day " + (TimeManagement.Day + 1).ToString();
    }
}
