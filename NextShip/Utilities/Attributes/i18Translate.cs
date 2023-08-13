using System;
using System.Collections.Generic;

namespace NextShip.Utilities.Attributes;

public class i18Translate : Attribute
{
    public static List<Translate> Translates;
}

public class Translate
{
    public string Key;
    private SupportedLangs StringLang;
    private string Text;
    private SupportedLangs[] TextSupportedLangs;

    public Dictionary<SupportedLangs, string> TranslateDictionary;

    public Translate(string key, string text, SupportedLangs stringLang, SupportedLangs[] supportedLangs)
    {
        Key = key;
        Text = text;
        StringLang = stringLang;
        TextSupportedLangs = supportedLangs;
    }

    public string Get(SupportedLangs lang = SupportedLangs.English)
    {
        return TranslateDictionary[lang] ?? string.Empty;
    }

    public void Add(string text, SupportedLangs lang)
    {
        TranslateDictionary[lang] = text;
    }
}