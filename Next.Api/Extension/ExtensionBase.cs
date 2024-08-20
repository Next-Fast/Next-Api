using HarmonyLib;

namespace Next.Api.Extension;

public abstract class ExtensionBase
{
    private Harmony _harmony => _Harmony;

    public abstract void Use();

    public abstract void UnUse();
}