using UnityEngine;

public class FirePit : WorldObject
{
    [SerializeField] private Container _container;
    [SerializeField] private float _cookRate;

    public Container Container => _container;

    public override void OnInteraction(PlayerController playerController)
    {
        playerController.OpenContainer(_container);
    }

    public override string GetInteractionPhrase()
    {
        return "Use Fire Pit";
    }

    private void Update()
    {
        if (_cookRate > 0)
        {
            float cookThisFrame = _cookRate * (TimeManagement.GameDeltaTime / 60f);
            int itemsCooking = _container.Items.Count;
            cookThisFrame /= itemsCooking;
            foreach (InventoryItemInstance item in _container.Items)
                item.Cook(cookThisFrame);
        }
    }
}
