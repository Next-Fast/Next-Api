using HarmonyLib;

namespace TheIdealShip.Patches;

[HarmonyPatch]
public class EOSManagerPatch
{
    [HarmonyPatch(typeof(AskToMergeGuest), nameof(AskToMergeGuest.Start))]
    [HarmonyPostfix]
    public static void AskToMergeGuest_Start_Postfix(AskToMergeGuest __instance)
    {
        EOSManager.Instance.EndMergeGuestAccountFlow();
        __instance.gameObject.SetActive(false);
    }
}