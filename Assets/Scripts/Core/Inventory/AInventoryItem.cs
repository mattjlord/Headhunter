using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AInventoryItem : ScriptableObject
{
    public string Name;
    public string Description;
    public float Weight;
    public Sprite Image;

    public virtual AInventoryItem? GetPerishedVersion() { return null; }
    public virtual AInventoryItem? GetCookedVersion() { return null; }
    public virtual List<ItemInteractionType> GetInteractionOptions() => new List<ItemInteractionType>() { ItemInteractionType.Discard };
}