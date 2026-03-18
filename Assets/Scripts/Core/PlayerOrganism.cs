using UnityEngine;

public class PlayerOrganism : Organism
{

    [SerializeField] private Shooting _shooting;
    [SerializeField] private Inventory _inventory;

    protected override void Start()
    {
        base.Start();
        _inventory.OnEncumbranceChanged += OnEncumbranceChanged;
    }

    public Shooting Shooting { get { return _shooting; } }
    public Inventory Inventory { get { return _inventory;} }

    private void OnEncumbranceChanged(float encumbrance)
    {
        float movementModifier = Mathf.Lerp(1, 0.5f, encumbrance);
        Movement.EncumbranceModifier = movementModifier;
    }
}
