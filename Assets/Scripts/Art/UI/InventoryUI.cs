using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private ItemDetails _itemDetails;

    private bool _enabled;

    private Image _menuFrame;
    private RectTransform _rectTransform;

    private void Start()
    {
        _menuFrame = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _highlight.SetActive(false);
        Enabled = false;
    }

    private void Update()
    {
        if (!_enabled)
            return;

        DisplayInventory();
    }

    public bool Enabled
    {
        get { return _enabled; }
        set 
        { 
            _enabled = value;
            _menuFrame.enabled = value;
            if (value == false)
                _highlight.SetActive(false);
        }
    }

    private void DisplayInventory()
    {
        Vector2 localMouse;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            Input.mousePosition,
            null, // null if Screen Space - Overlay
            out localMouse
        );

        UIUtils.DisplayContainer(
            _inventory.Container,
            _rectTransform,
            localMouse,
            out InventoryItem currentItem,
            out Vector2? currentItemPos
        );

        if (currentItem != null)
        {
            RectTransform highlightRect = _highlight.GetComponent<RectTransform>();

            highlightRect.anchoredPosition = currentItemPos.Value;
            _highlight.SetActive(true);

            _itemDetails.gameObject.SetActive(true);
            _itemDetails.ShowItem(localMouse, currentItem);
        }
        else
        {
            _highlight.SetActive(false);
            _itemDetails.gameObject.SetActive(false);
        }
    }
}
