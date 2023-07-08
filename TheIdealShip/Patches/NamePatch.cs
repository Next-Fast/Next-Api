using AmongUs.Data;
using AmongUs.Data.Legacy;
using HarmonyLib;

namespace TheIdealShip.Patches;

[Harmony]
public class AccountManagerPatch
{
    [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.RandomizeName))]
    public static class RandomizeNamePatch
    {
        private static bool Prefix(AccountManager __instance)
        {
            if (LegacySaveManager.lastPlayerName == null) return true;
            DataManager.player.Customization.Name = LegacySaveManager.lastPlayerName;
            __instance.accountTab.UpdateNameDisplay();
            return false;
        }
    }
}