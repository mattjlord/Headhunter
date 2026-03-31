using UnityEngine;

public class PlayerOrganism : Organism
{

    [SerializeField] private Shooting _shooting;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Crafting _crafting;

    protected override void Start()
    {
        base.Start();
        _inventory.OnEncumbranceChanged += OnEncumbranceChanged;
    }

    public Shooting Shooting { get { return _shooting; } }
    public Inventory Inventory { get { return _inventory;} }
    public Crafting Crafting { get { return _crafting; } }

    private void OnEncumbranceChanged(float encumbrance)
    {
        float movementModifier = Mathf.Lerp(1, 0.5f, encumbrance);
        Movement.EncumbranceModifier = movementModifier;
    }
}
