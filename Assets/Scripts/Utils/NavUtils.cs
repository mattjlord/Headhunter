public static class NavUtils
{
    public static int GetNavMeshID(OrganismType organismType)
    {
        switch (organismType)
        {
            case OrganismType.Mudyak:
                return -1372625422;
            case OrganismType.BulletRaptor:
                return -334000983;
            default:
                return 0;
        }
    }
}