using UnityEngine;

public class PlayerOrganism : Organism
{
    [SerializeField] private Shooting _shooting;

    public Shooting Shooting { get { return _shooting; } }
}
