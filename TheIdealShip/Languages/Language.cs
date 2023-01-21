using csv = TheIdealShip.Languages.LanguageCSV;
using pack = TheIdealShip.Languages.LanguagePack;
using System.Collections.Generic;
using System;
using System.IO;

namespace TheIdealShip.Languages;

public static class Language
{
    public static string GetString(string s, Dictionary<string, string> replacementDic = null)
    {
        var langId = TranslationController.InstanceExists ? TranslationController.Instance.currentLanguage.languageID : SupportedLangs.English;
        string str = "";
        if (File.Exists(@"Language\"+LanguagePack.languageName+".dat"))
        {
            str = LanguagePack.GetPString(s);
        }
        else
        {
            str = csv.GetCString(s,langId);
        }
        if (replacementDic != null)
        {
            foreach (var rd in replacementDic)
            {
                str = str.Replace(rd.Key, rd.Value);
            }
        }
        return str;
    }
    public static void Init()
    {
        if (!(File.Exists(@"Language\" + LanguagePack.languageName + ".dat")))
        {
            csv.LoadCSV();
        }
        else
        {
            pack.Load();
        }
    }
}