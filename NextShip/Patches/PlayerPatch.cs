using HarmonyLib;

namespace NextShip.Patches;

[Harmony]
public static class PlayerPatch
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Awake))]
    public static void PlayerControlAwake_PostfixPatch(PlayerControl __instance)
    {
    }
}