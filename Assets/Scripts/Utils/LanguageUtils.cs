public static class LanguageUtils
{
    public static string GetSenseVerb(SenseType senseType)
    {
        switch (senseType)
        {
            case SenseType.Sight:
                return "sees";
            case SenseType.Sound:
                return "hears";
            default:
                return "smells";
        }
    }

    public static string GetInteractionPhrase(InventoryItemInstance instance, ItemInteractionType interactionType)
    {
        switch (interactionType)
        {
            case ItemInteractionType.Discard:
                return "Discard";
            case ItemInteractionType.Consume:
                return "Consume";
            case ItemInteractionType.Equip:
                if (instance.EquipmentSlot == null)
                    return "Equip";
                else
                    return "Unequip";
            default:
                return "";
        }
    }
}