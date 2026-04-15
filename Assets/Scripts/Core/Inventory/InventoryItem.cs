using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem : ScriptableObject
{
    public string Name;
    public string Description;
    public float Weight;
    public Sprite Image;

    public virtual InventoryItem? GetPerishedVersion() { return null; }
    public virtual InventoryItem? GetCookedVersion() { return null; }
    public virtual List<ItemInteractionType> GetInteractionOptions() => new List<ItemInteractionType>() { ItemInteractionType.Discard };

    public virtual string GetTypeName() => "Generic Item";
}