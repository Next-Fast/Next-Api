using HarmonyLib;

namespace NextShip.Api;

internal static class APIHarmony
{
    internal static Harmony _Harmony { get; set; }

    static APIHarmony()
    {
        _Harmony = new Harmony("net.NextShip.Api");
    }
}