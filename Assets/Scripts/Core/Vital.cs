using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Vital
{
    [SerializeField, Range(0f, 100f)] private float _value;
    [SerializeField, Min(0f)] private float _passiveIncreaseRate; // Per minute of game time
    [SerializeField, Min(0f)] private float _increaseSensitivity;
    [SerializeField, Min(0f)] private float _decreaseSensitivity;

    public float Value { get { return _value; } }

    public void Update()
    {
        if (_passiveIncreaseRate > 0)
        {
            float increaseThisFrame = _passiveIncreaseRate * (Time.deltaTime / 60f);
            IncreaseValue(increaseThisFrame);
        }
    }

    public void IncreaseValue(float value, bool ignoreSensitivity = false)
    {
        float m = 1f;
        if (!ignoreSensitivity)
        {
            m = _increaseSensitivity;
        }
        _value += (m * value);
        _value = Mathf.Clamp(_value, 0, 100);
    }

    public void DecreaseValue(float value, bool ignoreSensitivity = false)
    {
        float m = 1f;
        if (!ignoreSensitivity)
        {
            m = _decreaseSensitivity;
        }
        _value -= (m * value);
        _value = Mathf.Clamp(_value, 0, 100);
    }

    public void SetIncreaseSensitivity(float value)
    {
        if (value > 0)
        {
            _increaseSensitivity = value;
        }
    }

    public void SetDecreaseSensitivity(float value)
    {
        if (value > 0)
        {
            _decreaseSensitivity = value;
        }
    }

    public void SetBothSensitivities(float value)
    {
        SetIncreaseSensitivity(value);
        SetDecreaseSensitivity(value);
    }

    public void ResetIncreaseSensitivity()
    {
        _increaseSensitivity = 1.0f;
    }

    public void ResetDecreaseSensitivity()
    {
        _decreaseSensitivity = 1.0f;
    }

    public void ResetBothSensitivities()
    {
        ResetIncreaseSensitivity();
        ResetDecreaseSensitivity();
    }
}
