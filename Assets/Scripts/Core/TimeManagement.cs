using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    private static int _day;
    private static int _hours;
    private static int _minutes;

    private float _totalDays = 0;
    private float _totalHours = 0;
    private float _totalMinutes = 0;

    private float _startTime;

    public static int Day { get { return _day; } }
    public static int Hours { get { return _hours; } }
    public static int Minutes { get { return _minutes; } }

    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        UpdateTime();
        UpdateLighting();
        ProcessEvents();
    }

    private void UpdateTime()
    {
        _totalMinutes = Time.time - _startTime;
        _totalHours = _totalMinutes / 60f;
        _totalDays = _totalHours / 15;

        _day = Mathf.FloorToInt(_totalDays);
        _hours = Mathf.FloorToInt(_totalHours) % 15;
        _minutes = Mathf.FloorToInt(_totalMinutes) % 60;
    }

    private void UpdateLighting()
    {

    }

    private void ProcessEvents()
    {

    }
}
