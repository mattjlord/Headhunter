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
    [SerializeField] private TMP_Text _durability;

    [SerializeField] private Image _image;

    [SerializeField] private RectTransform _rectTransform;

    public void ShowItem(Vector2 mousePos, InventoryItemInstance item)
    {
        _rectTransform.anchoredPosition = mousePos;

        _name.text = item.Item.Name;
        _description.text = item.Item.Description;
        _weight.text = item.Item.Weight.ToString() + " lb";
        _image.sprite = item.Item.Image;

        float durability = item.Durability;
        if (durability == 100)
            _durability.text = "";
        else
        {
            _durability.text = ((int)durability).ToString() + "%";
            _durability.color = Color.Lerp(Color.red, Color.green, durability / 100);
        }
    }
}
