using UnityEngine;

[CreateAssetMenu(fileName = "Mudyak Coat", menuName = "Inventory Item/Mudyak Coat")]
public class MudyakCoat : AEquippable
{
    public GameObject mudyakStimPrefab;

    public override void OnEquip(PlayerOrganism organism)
    {
        GameObject instance = Instantiate(mudyakStimPrefab, organism.transform.position, Quaternion.identity);
        instance.name = "Mudyak Stimulus";
        instance.transform.SetParent(organism.transform);
        organism.Visibility.OverrideStimulus = instance.GetComponent<Stim_Mudyak>();
        organism.Odor.OverrideOrganismType = OrganismType.Mudyak;
        instance.GetComponent<Stim_Mudyak>().AssociatedObject = organism;
    }

    public override void OnUnequip(PlayerOrganism organism)
    {
        Destroy(organism.transform.Find("Mudyak Stimulus").gameObject);
        organism.Visibility.OverrideStimulus = null;
        organism.Odor.OverrideOrganismType = null;
    }
}