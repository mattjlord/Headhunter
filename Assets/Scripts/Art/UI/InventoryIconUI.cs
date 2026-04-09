using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryIconUI : MonoBehaviour
{
    [SerializeField] private Image _image;

    public bool Enabled
    {
        set
        {
            _image.enabled = value;
        }
    }

    public InventoryItemInstance Item
    {
        set
        {
            _image.sprite = value.Item.Image;
            Enabled = true;
        }
    }
}
