using HarmonyLib;
using Hazel;
using NextShip.Api.Enums;
using NextShip.Api.RPCs;
using NextShip.Manager;

namespace NextShip.Patches;

[Harmony]
public static class PlayerPatch
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Awake))]
    [HarmonyPostfix]
    public static void PlayerControlAwake_PostfixPatch(PlayerControl __instance)
    {
        if (NextPlayerManager.Instance.TryGetPlayer(__instance, out _))
            return;
    }

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
    [HarmonyPostfix]
    public static void OnPlayerJoined()
    {
        FastRpcWriter.StartNew((byte)SystemRPCFlag.VersionCheck, SendOption.Reliable);
    }
}