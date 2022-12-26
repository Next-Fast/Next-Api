using static TheIdealShip.Languages.LanguageCSV;
using static TheIdealShip.Languages.LanguagePack;
using System.Collections.Generic;

namespace TheIdealShip.Languages;

public static class Language
{
    public static string GetString(string s, Dictionary<string, string> replacementDic = null)
    {
        var langId = TranslationController.InstanceExists ? TranslationController.Instance.currentLanguage.languageID : SupportedLangs.English;
        string str = "";
//        if (HPack)
//        {
//            str = GetPString(s);
//        }
//        else
//        {
            str = GetCString(s,langId);
//        }
        if (replacementDic != null)
            foreach (var rd in replacementDic)
            {
                str = str.Replace(rd.Key, rd.Value);
            }
        return str;
    }
}