using UnityEngine;
using UnityEngine.UI;

public class InventoryIconUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _equippedIndicator;
    [SerializeField] private RectTransform _cookProgress;

    private InventoryItemInstance _item;

    public bool Enabled
    {
        set
        {
            _image.enabled = value;
            if (value == true)
            {
                _equippedIndicator.enabled = _item.EquipmentSlot != null;

                float cookProgress = _item.CookProgress / 100f;
                float topValue = Mathf.Lerp(25, 0, cookProgress);
                Vector2 offsetMax = _cookProgress.offsetMax;
                offsetMax.y = -topValue;
                _cookProgress.offsetMax = offsetMax;
            }
            else
            {
                _equippedIndicator.enabled = false;
                Vector2 offsetMax = _cookProgress.offsetMax;
                offsetMax.y = -25f;
                _cookProgress.offsetMax = offsetMax;
            }
        }
    }

    public InventoryItemInstance Item
    {
        set
        {
            _image.sprite = value.Item.Image;
            _item = value;
            Enabled = true;
        }
    }
}
