using HarmonyLib;

namespace NextShip.Patches;

public static class ModManagerPatch
{
    [HarmonyPatch(typeof(ModManager), nameof(ModManager.Awake)), HarmonyPostfix]
    public static void ModManager_AwakePatch(ModManager __instance) => __instance.ShowModStamp();
}