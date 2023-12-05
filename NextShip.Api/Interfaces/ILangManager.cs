namespace NextShip.Api.Interfaces;

public interface ILangManager
{
    public void RegisterLang(ILang lang);
    
    public void UnRegisterLang(ILang lang);

    public void SetLang(SupportedLangs langId, ILang lang);
}