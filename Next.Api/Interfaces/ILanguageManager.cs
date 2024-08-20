using Next.Api.Bases;

namespace Next.Api.Interfaces;

public interface ILanguageManager
{
    public void AddLoader(LanguageLoaderBase _loader);

    public LanguageLoaderBase? GetLoader(string extensionName);

    public void AddToMap(SupportedLangs lang, string key, string value, string loaderName);
}