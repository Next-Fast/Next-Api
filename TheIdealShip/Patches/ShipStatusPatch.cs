using HarmonyLib;

namespace TheIdealShip.Patches;

[HarmonyPatch(typeof(GameManager), nameof(GameManager.CheckTaskCompletion))]
class CheckTaskCompletionPatch
{
    public static bool Prefix(ref bool __result)
    {
        return true;
    }
}