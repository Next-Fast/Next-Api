using Next.Api.Interfaces;

namespace Next.Api.Bases;

public abstract class LanguageLoaderBase(string[] filter)
{
    public string[] Filter { get; protected set; } = filter;

    public abstract void Load(ILanguageManager _manager, Stream stream, string FileName);
}