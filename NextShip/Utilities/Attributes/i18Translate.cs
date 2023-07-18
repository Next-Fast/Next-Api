using System;
using System.Collections.Generic;

namespace NextShip.Utilities.Attributes;

public class i18Translate : Attribute
{
    public static List<Translate> Translates;

    public i18Translate()
    {
    }
}

public class Translate
{
    public string Key;
    private string Text;
    private SupportedLangs StringLang;
    private SupportedLangs[] TextSupportedLangs;

    public Dictionary<SupportedLangs, string> TranslateDictionary;

    public Translate(string key,string text, SupportedLangs stringLang, SupportedLangs[] supportedLangs)
    {
        Key = key;
        Text = text;
        StringLang = stringLang;
        TextSupportedLangs = supportedLangs;
    }

    public string Get(SupportedLangs lang = SupportedLangs.English) => TranslateDictionary[lang] ?? String.Empty;

    public void Add(string text, SupportedLangs lang) => TranslateDictionary[lang] = text;
}