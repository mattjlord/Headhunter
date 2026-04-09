using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlHintsUI : MonoBehaviour
{
    [SerializeField] private Image _lmbIcon;
    [SerializeField] private TMP_Text _lmbText;
    [SerializeField] private Image _mWheelIcon;
    [SerializeField] private TMP_Text _mWheelText;
    [SerializeField] private Image _rmbIcon;
    [SerializeField] private TMP_Text _rmbText;

    public string LMBText
    {
        set
        {
            _lmbIcon.enabled = value != "";
            _lmbText.text = value;
        }
    }

    public string MWheelText
    {
        set
        {
            _mWheelIcon.enabled = value != "";
            _mWheelText.text = value;
        }
    }

    public string RMBText
    {
        set
        {
            _rmbIcon.enabled = value != "";
            _rmbText.text = value;
        }
    }

    public void HideAll()
    {
        LMBText = "";
        MWheelText = "";
        RMBText = "";
    }
}
