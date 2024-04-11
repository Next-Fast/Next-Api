using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

#nullable enable
public class NextLangManager : ILangManager
{
    private readonly List<ILang> AllLang = [];

    private readonly Dictionary<SupportedLangs, ILang> Current = new();

    public NextLangManager()
    {
        DataManager.Settings.Language.OnLanguageChanged += (Action)(() =>
        {
            SetCurrentLang(DataManager.Settings.Language.CurrentLanguage);
        });
    }

    public SupportedLangs CurrentLang { get; private set; }

    public void RegisterLang(ILang lang)
    {
        if (!Current.ContainsKey(lang.LangId))
            Set(lang.LangId, lang);

        AllLang.Add(lang);
        lang.Init();
    }

    public void UnRegisterLang(ILang lang)
    {
        if (Current[lang.LangId] == lang)
            Current.Remove(lang.LangId);

        AllLang.Remove(lang);
        lang.Clear();
    }

    public void Set(SupportedLangs langId, ILang lang)
    {
        Current[langId] = lang;
    }

    public void SetCurrentLang(SupportedLangs langId)
    {
        CurrentLang = langId;
    }

    public ILang GetCurrentLang()
    {
        return Current[CurrentLang];
    }

    public ILang GetLang(SupportedLangs langId)
    {
        return Current[langId];
    }

    public ILang GetLang(string LangNameOrAuthor)
    {
        return AllLang.FirstOrDefault(n => n.LangName == LangNameOrAuthor || n.Author == LangNameOrAuthor)!;
    }

    public ILang[] GetLangS(SupportedLangs langId)
    {
        return AllLang.FindAll(n => n.LangId == langId).ToArray();
    }

    public ILang[] GetLangS(string name)
    {
        return AllLang.FindAll(n => n.LangName == name || n.Author == name).ToArray();
    }
}