using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private Crafting _crafting;
    [SerializeField] private ContainerUI _inputsUI;
    [SerializeField] private ContainerUI _outputsUI;
    [SerializeField] private Button _craftButton;

    private bool _enabled = false;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _inputsUI.Enabled = value;
            _outputsUI.Enabled = value;
            _craftButton.gameObject.SetActive(value);
            GetComponent<Image>().enabled = value;
            _enabled = value;
        }
    }

    private void Start()
    {
        _inputsUI.Container = _crafting.Inputs;
        _outputsUI.Container = _crafting.Outputs;

        _crafting.Inputs.OnContentsChanged += UpdateButton;

        _craftButton.onClick.AddListener(Craft);
    }

    public InventoryItemInstance CurrentInputItem { get => _inputsUI.CurrentItem; }
    public InventoryItemInstance CurrentOutputItem { get => _outputsUI.CurrentItem; }

    private void UpdateButton()
    {
        _craftButton.interactable = _crafting.CanCraft;
    }

    private void Craft()
    {
        _crafting.Craft();
    }
}
