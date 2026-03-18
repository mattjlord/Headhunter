using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TimeManagement : MonoBehaviour
{
    [SerializeField] private int _hourOffset;
    [SerializeField] private AnimationCurve _lightCurve;

    [SerializeField] private float _minTemp;
    [SerializeField] private float _maxTemp;
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;

    [SerializeField] private Transform _sunTransform;
    [SerializeField] private HDAdditionalLightData _lightData;

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
        _totalMinutes = Time.time - _startTime + (60 * _hourOffset);
        _totalHours = _totalMinutes / 60f;
        _totalDays = _totalHours / 15;

        _day = Mathf.FloorToInt(_totalDays);
        _hours = Mathf.FloorToInt(_totalHours) % 15;
        _minutes = Mathf.FloorToInt(_totalMinutes) % 60;
    }

    private void UpdateLighting()
    {
        float dayProgress = _totalDays % 1;
        float sunAngle = Mathf.Lerp(-90, 270, dayProgress);
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
