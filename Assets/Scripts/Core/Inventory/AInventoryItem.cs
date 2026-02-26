using System;
using UnityEngine;

[Serializable]
public abstract class AInventoryItem : ScriptableObject
{
    public string Name;
    public string Description;
    public float Weight;
    public Sprite Image;
}