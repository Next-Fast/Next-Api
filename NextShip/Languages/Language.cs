global using static NextShip.Languages.Language;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using csv = NextShip.Languages.LanguageCSV;
using pack = NextShip.Languages.LanguagePack;

namespace NextShip.Languages;

public static class Language
{
    public static string GetString(string s, Dictionary<string, string>? replacementDic = null)
    {
        var langId = TranslationController.InstanceExists
            ? TranslationController.Instance.currentLanguage.languageID
            : SupportedLangs.English;
        var str = "";
        str = File.Exists(@"Language\" + pack.languageName + ".dat") ? pack.GetPString(s) : csv.GetCString(s, langId);
        return replacementDic == null
            ? str
            : replacementDic.Aggregate(str, (current, rd) => current.Replace(rd.Key, rd.Value));
    }

    public static void Init()
    {
        if (!File.Exists(@"Language\" + pack.languageName + ".dat"))
            csv.LoadCSV();
        else
            pack.Load();
    }
}