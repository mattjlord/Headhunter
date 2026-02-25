using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _weight;

    [SerializeField] private Image _image;

    [SerializeField] private RectTransform _rectTransform;

    public void ShowItem(Vector2 mousePos, InventoryItem item)
    {
        _rectTransform.anchoredPosition = mousePos;

        _name.text = item.Name;
        _description.text = item.Description;
        _weight.text = item.Weight.ToString() + " lb";
        _image.sprite = item.Image;
    }
}
