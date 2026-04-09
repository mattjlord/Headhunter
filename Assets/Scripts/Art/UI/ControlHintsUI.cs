using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlHintsUI : MonoBehaviour
{
    [SerializeField] private GameObject _lmb;
    [SerializeField] private GameObject _mWheel;
    [SerializeField] private GameObject _rmb;

    public string LMBText
    {
        set
        {
            bool active = value != "";
            _lmb.SetActive(active);
            if (active)
            {
                _lmb.GetComponentInChildren<Image>().enabled = true;
                _lmb.GetComponentInChildren<TMP_Text>().text = value;
            }
        }
    }

    public string MWheelText
    {
        set
        {
            bool active = value != "";
            _mWheel.SetActive(active);
            if (active)
            {
                _mWheel.GetComponentInChildren<Image>().enabled = true;
                _mWheel.GetComponentInChildren<TMP_Text>().text = value;
            }
        }
    }

    public string RMBText
    {
        set
        {
            bool active = value != "";
            _rmb.SetActive(active);
            if (active)
            {
                _rmb.GetComponentInChildren<Image>().enabled = true;
                _rmb.GetComponentInChildren<TMP_Text>().text = value;
            }
        }
    }

    public void HideAll()
    {
        LMBText = "";
        MWheelText = "";
        RMBText = "";
    }
}
