using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    [SerializeField] protected Container container;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _itemIconPrefab;
    [SerializeField] private ItemDetails _itemDetails;

    private bool _enabled;

    private Image _menuFrame;
    private RectTransform _rectTransform;

    private Image[][] _iconGrid;

    private InventoryItemInstance? _currentItem;

    public Container Container { 
        set { container = value; } 
        get { return container; }
    }

    public InventoryItemInstance? CurrentItem { get { return _currentItem; } }

    private Image MenuFrame
    {
        get
        {
            if (_menuFrame == null)
                _menuFrame = GetComponent<Image>();
            return _menuFrame;
        }
    }

    protected virtual void Start()
    {
        _menuFrame = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _highlight.SetActive(false);
        _iconGrid = UIUtils.InitIconGrid(_rectTransform, _itemIconPrefab);
    }

    protected virtual void Update()
    {
        if (!_enabled)
            return;

        DisplayContainer();
    }

    public bool Enabled
    {
        get { return _enabled; }
        set
        {
            _enabled = value;
            MenuFrame.enabled = value;
            if (value == false)
            {
                _highlight.SetActive(false);
                _itemDetails.gameObject.SetActive(false);
                if (_iconGrid != null)
                {
                    foreach (Image[] row in _iconGrid)
                    {
                        foreach (Image image in row)
                        {
                            image.enabled = false;
                        }
                    }
                }
            }
            EnabledExtension(value);
        }
    }

    protected virtual void EnabledExtension(bool value) { }

    private void DisplayContainer()
    {
        Vector2 localMouse;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            Input.mousePosition,
            null, // null if Screen Space - Overlay
            out localMouse
        );

        UIUtils.DisplayContainerContents(
            container,
            _rectTransform,
            localMouse,
            _iconGrid,
            out InventoryItemInstance currentItem,
            out Vector2? currentItemPos
        );

        if (currentItem != null && currentItemPos != null)
        {
            RectTransform highlightRect = _highlight.GetComponent<RectTransform>();

            highlightRect.anchoredPosition = currentItemPos.Value;
            _highlight.SetActive(true);

            _itemDetails.gameObject.SetActive(true);
            _itemDetails.ShowItem(localMouse, currentItem);

            _currentItem = currentItem;
        }
        else
        {
            _highlight.SetActive(false);
            _itemDetails.gameObject.SetActive(false);
            _currentItem = null;
        }
    }
}
