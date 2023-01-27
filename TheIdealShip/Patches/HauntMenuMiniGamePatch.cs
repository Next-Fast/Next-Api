using HarmonyLib;
using TheIdealShip.Utilities;

namespace TheIdealShip.Patches
{
    public class HauntMenuMiniGamePatch
    {
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.Start))]
        public static bool Prefix(HauntMenuMinigame __instance)
        {
            if (CustomOptionHolder.disableHauntMenu.getBool())
            {
                DisableHM(__instance);
                return false;
            }

            return false;
        }

        public static void DisableHM(HauntMenuMinigame __instance)
        {
            __instance.gameObject.SetActive(false);
            __instance.enabled = false;
            for (var i = 0; i <= __instance.FilterButtons.Count ; i++)
            {
                __instance.FilterButtons[1].gameObject.SetActive(false);
            }
        }
    }
    [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.FixedUpdate))]
    class HauntMenuMiniGameFixedUpdatePatch
    {
        public static bool Prefix(HauntMenuMinigame __instance)
        {
            if (CustomOptionHolder.disableHauntMenu.getBool())
            {
                HauntMenuMiniGamePatch.DisableHM(__instance);
                return false;
            }

            return false;
        }

        public static void Postfix(HauntMenuMinigame __instance)
        {
            if (CustomOptionHolder.disableHauntMenu.getBool() || CachedPlayer.LocalPlayer.PlayerControl.IsSurvival())
            {
                __instance.gameObject.SetActive(false);
            }
        }

    }

    class HauntMenuMiniGameBeginPatch
    {
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.Begin))]
        public static bool Prefix(HauntMenuMinigame __instance)
        {
            if (CustomOptionHolder.disableHauntMenu.getBool())
            {
                HauntMenuMiniGamePatch.DisableHM(__instance);
                return false;
            }

            return false;
        }
    }

    class HauntMenuMinigameClose
    {
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.Close))]
        public static bool Prefix(HauntMenuMinigame __instance)
        {
            if (CustomOptionHolder.disableHauntMenu.getBool())
            {
                HauntMenuMiniGamePatch.DisableHM(__instance);
                return false;
            }

            return false;
        }
    }
}