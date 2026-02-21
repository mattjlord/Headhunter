using UnityEngine;

public class MasterOrganismManager : MonoBehaviour
{
    private OrganismManager _mudyakManager;
    private OrganismManager _bulletRaptorManager;

    public Organism SpawnOrganism(OrganismType organismType, Vector2 point)
    {
        switch (organismType)
        {
            case OrganismType.Mudyak:
                return _mudyakManager.SpawnOrganism(point);
            case OrganismType.BulletRaptor:
                return _bulletRaptorManager.SpawnOrganism(point);
            default:
                return null;
        }
    }
}
