using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalsUI : MonoBehaviour
{
    [SerializeField] private Vitals _vitals;
    [SerializeField] private Slider _hunger;
    [SerializeField] private Slider _thirst;
    [SerializeField] private Slider _exhaustion;
    [SerializeField] private Slider _heat;
    [SerializeField] private Slider _injury;
    [SerializeField] private Slider _toxicity;

    // Update is called once per frame
    void Update()
    {
        _hunger.value = _vitals.GetVital(VitalType.Hunger).Value / 100;
        _thirst.value = _vitals.GetVital(VitalType.Thirst).Value / 100;
        _exhaustion.value = _vitals.GetVital(VitalType.Exhaustion).Value / 100;
        _heat.value = _vitals.GetVital(VitalType.Heat).Value / 100;
        _injury.value = _vitals.GetVital(VitalType.Injury).Value / 100;
        _toxicity.value = _vitals.GetVital(VitalType.Toxicity).Value / 100;
    }
}
