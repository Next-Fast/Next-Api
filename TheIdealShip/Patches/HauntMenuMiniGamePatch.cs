using HarmonyLib;

namespace TheIdealShip.Patches
{
    public class HauntMenuMiniGamePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.Start))]
        public static bool StartPatch()
        {
            if (CustomOptionHolder.disableHauntMenu.getBool()) return false;

            return true;
        }
    }
}