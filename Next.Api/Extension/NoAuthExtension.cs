using HarmonyLib;

namespace Next.Api.Extension;

[Harmony]
public static class NoAuthExtension
{
    public static void Use()
    {
        _Harmony.PatchAll(typeof(NoAuthExtension));
    }

    [HarmonyPatch(typeof(AuthManager._CoConnect_d__4), nameof(AuthManager._CoConnect_d__4.MoveNext))]
    [HarmonyPatch(typeof(AuthManager._CoWaitForNonce_d__6), nameof(AuthManager._CoWaitForNonce_d__6.MoveNext))]
    [HarmonyPrefix]
    private static bool NoAuthPatch(ref bool __result)
    {
        if (ServerUtils.CurrentServer.IsVanilla())
            return true;
        
        __result = false;
        return false;
    }
}