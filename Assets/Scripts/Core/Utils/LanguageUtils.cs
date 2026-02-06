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
}