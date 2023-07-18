using HarmonyLib;

namespace NextShip.Patches;

[HarmonyPatch(typeof(GameManager), nameof(GameManager.CheckTaskCompletion))]
internal class CheckTaskCompletionPatch
{
    public static bool Prefix(ref bool __result)
    {
        return true;
    }
}