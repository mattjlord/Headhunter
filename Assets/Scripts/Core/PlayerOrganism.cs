using UnityEngine;

public class PlayerOrganism : Organism
{
    [SerializeField] private Shooting _shooting;
    [SerializeField] private Inventory _inventory;

    public Shooting Shooting { get { return _shooting; } }
    public Inventory Inventory { get { return _inventory;} }
}
