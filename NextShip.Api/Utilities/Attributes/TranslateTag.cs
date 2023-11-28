namespace NextShip.Api.Utilities.Attributes;

public class TranslateTag : Attribute
{
    public static int AllCount;
    public readonly int Count;

    public readonly string Tag;

    public Dictionary<SupportedLangs, string> Translate = new();
    public SupportedLangs VanillaLang;


    public TranslateTag(string tag = "None", SupportedLangs Lang = 0, string VanillaText = "")
    {
        Tag = tag;
        VanillaLang = 0;
        Count = AllCount;
        AllCount++;

        if (tag == "None") Tag = $"None Count{Count}";
        if (VanillaText != "") Translate[VanillaLang] = VanillaText;
    }

    public string this[int value] => value < 0 ? Tag : Translate[(SupportedLangs)value];

    public static explicit operator string(TranslateTag translateTag)
    {
        return translateTag.Tag;
    }

    public static void Registration(Type type)
    {
    }
}