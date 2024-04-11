namespace NextShip.Api.Interfaces;

public interface ILangManager
{
    public void RegisterLang(ILang lang);

    public void UnRegisterLang(ILang lang);

    public void Set(SupportedLangs langId, ILang lang);

    public void SetCurrentLang(SupportedLangs langId);

    public ILang GetCurrentLang();

    public ILang GetLang(SupportedLangs langId);

    public ILang GetLang(string LangNameOrAuthor);

    public ILang[] GetLangS(SupportedLangs langId);

    public ILang[] GetLangS(string name);
}