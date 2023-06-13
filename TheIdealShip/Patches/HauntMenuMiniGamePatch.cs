using AmongUs.GameOptions;
using HarmonyLib;
using TheIdealShip.Utilities;
using TheIdealShip.Roles;

namespace TheIdealShip.Patches
{
    /* public class HauntMenuMiniGamePatch
    {
        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.Start))]
        public static bool Prefix(HauntMenuMinigame __instance)
        {
            if (CustomOptionHolder.disableHauntMenu.getBool()) return false;

            return true;
        }
    }
    [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.FixedUpdate))]
    class HauntMenuMiniGameFixedUpdatePatch
    {
        public static bool Prefix(HauntMenuMinigame __instance)
        {
            if (CustomOptionHolder.disableHauntMenu.getBool()) return false;

            return false;
        }

        public static void Postfix(HauntMenuMinigame __instance)
        {
            if (CustomOptionHolder.disableHauntMenu.getBool())
            {
                __instance.gameObject.SetActive(false);
                return;
            }
            if (CachedPlayer.LocalPlayer.PlayerControl.IsSurvival())
            {
                __instance.gameObject.SetActive(false);
            }
            else
            {
                __instance.gameObject.SetActive(true);
            }
            foreach (var b in __instance.FilterButtons)
            {
                b.gameObject.SetActive(false);
            }
        }
    } */

    [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.Refresh))]
    public static class AbilityButtonRefreshPatch
    {
        public static void Postfix(AbilityButton __instance)
        {
            if(!CustomOptionHolder.disableHauntMenu.getBool()) return;
            __instance.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.SetFilterText))]
    public static class HauntMenuMinigameSetFilterTextPatch
    {
/*         public static bool Prefix(HauntMenuMinigame __instance)
        {
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek) return true;
            var roleText = RoleHelpers.GetRoleInfo(__instance.HauntTarget,false).name;
            var modifierText = RoleHelpers.GetRoleInfo(__instance.HauntTarget, true).name;

            string FilterText = roleText;
            if (modifierText != null) FilterText += ("\n" + modifierText);
            __instance.FilterText.text = FilterText;
            return false;
        } */
    }
}