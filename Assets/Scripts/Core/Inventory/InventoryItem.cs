using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private float _weight;
    [SerializeField] private Sprite _image;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public float Weight { get => _weight; }
    public Sprite Image { get => _image; }
}