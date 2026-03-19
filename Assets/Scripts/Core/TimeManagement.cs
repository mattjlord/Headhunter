using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static UnityEngine.GraphicsBuffer;

public enum TimeOfDay
{
    Night,
    Morning,
    HighSun,
    Afternoon
}

public class TimeManagement : MonoBehaviour
{
    [SerializeField] private int _hourOffset;
    [SerializeField] private AnimationCurve _sunRotationCurve;
    [SerializeField] private AnimationCurve _lightCurve;

    [SerializeField] private float _minTemp;
    [SerializeField] private float _maxTemp;
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;

    [SerializeField] private float _timeMultiplier = 1f;

    private static int _hoursPerDay = 15;
    private static int _highSunStart = 7;
    private static int _highSunEnd = 8;
    private static int _nightStart = 13;
    private static int _nightEnd = 2;

    [SerializeField] private Transform _sunTransform;
    [SerializeField] private HDAdditionalLightData _lightData;

    private static TimeOfDay _timeOfDay;

    private static int _day;
    private static int _hours;
    private static int _minutes;

    private static float _totalDays = 0;
    private static float _totalHours = 0;
    private static float _totalMinutes = 0;

    private float _startTime;

    private static float _minuteOffset = 0f;

    public static int Day { get { return _day; } }
    public static int Hours { get { return _hours; } }
    public static int Minutes { get { return _minutes; } }
    public static TimeOfDay TimeOfDay { get { return _timeOfDay; } }

    public static float SkipToNextSafePeriod()
    {
        if (_timeOfDay == TimeOfDay.Morning || _timeOfDay == TimeOfDay.HighSun)
            return SkipToTimeOfDay(TimeOfDay.Afternoon);
        else
            return SkipToTimeOfDay(TimeOfDay.Morning);
    }

    private static float SkipToTimeOfDay(TimeOfDay target)
    {
        int targetHour = 0;

        switch (target)
        {
            case TimeOfDay.Morning:
                targetHour = _nightEnd;
                break;
            case TimeOfDay.Afternoon:
                targetHour = _highSunEnd;
                break;
            default:
                return 0; // high sun and night are not skip targets
        }

        // Current absolute minutes (including previous skips)
        float currentMinutes = _day * _hoursPerDay * 60 + _hours * 60 + _minutes;

        // Target minutes today
        float targetMinutes = _day * _hoursPerDay * 60 + targetHour * 60;

        // If target already passed today, move to next day
        if (currentMinutes >= targetMinutes)
            targetMinutes += _hoursPerDay * 60;

        float minutesToSkip = targetMinutes - currentMinutes;

        _minuteOffset += minutesToSkip;

        return minutesToSkip;
    }

    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        UpdateTime();
        UpdateTimeOfDay();
        UpdateLighting();
        ProcessEvents();
    }

    private void UpdateTime()
    {
        _totalMinutes = (Time.time * _timeMultiplier) - _startTime + (60 * _hourOffset) + _minuteOffset;
        _totalHours = _totalMinutes / 60f;
        _totalDays = _totalHours / _hoursPerDay;

        _day = Mathf.FloorToInt(_totalDays);
        _hours = Mathf.FloorToInt(_totalHours) % _hoursPerDay;
        _minutes = Mathf.FloorToInt(_totalMinutes) % 60;
    }

    private void UpdateTimeOfDay()
    {
        if (_hours >= _nightEnd && _hours < _highSunStart)
            _timeOfDay = TimeOfDay.Morning;
        else if (_hours >= _highSunStart && _hours < _highSunEnd)
            _timeOfDay = TimeOfDay.HighSun;
        else if (_hours >= _highSunEnd && _hours < _nightStart)
            _timeOfDay = TimeOfDay.Afternoon;
        else
            _timeOfDay = TimeOfDay.Night;
    }

    private void UpdateLighting()
    {
        float dayProgress = _totalDays % 1;
        float rotationLerp = _sunRotationCurve.Evaluate(dayProgress);
        float sunAngle = Mathf.Lerp(-90, 270, rotationLerp);
        _sunTransform.rotation = Quaternion.Euler(new Vector3(sunAngle, 0, 0));

        float lightProgress = _lightCurve.Evaluate(dayProgress);
        float temp = Mathf.Lerp(_minTemp, _maxTemp, lightProgress);
        float intensity = Mathf.Lerp(_minIntensity, _maxIntensity, lightProgress);
        _lightData.intensity = intensity;
        _lightData.SetColor(Color.white, temp);
    }

    private void ProcessEvents()
    {

    }
}
