using UnityEngine;

[CreateAssetMenu(fileName = "Mudyak Coat", menuName = "Inventory Item/Mudyak Coat")]
public class MudyakCoat : AEquippable
{
    public GameObject mudyakStimPrefab;

    public override void OnEquip(PlayerOrganism organism)
    {
        GameObject instance = Instantiate(mudyakStimPrefab, organism.transform);
        instance.GetComponent<Visibility>().Organism = organism;
        instance.name = "Mudyak Stimulus";
        organism.Visibility.OverrideStimulus = instance.GetComponent<Stim_Mudyak>();
        organism.Odor.OverrideOrganismType = OrganismType.Mudyak;
    }

    public override void OnUnequip(PlayerOrganism organism)
    {
        Destroy(organism.transform.Find("Mudyak Stimulus").gameObject);
        organism.Visibility.OverrideStimulus = null;
        organism.Odor.OverrideOrganismType = null;
    }
}