using HarmonyLib;

namespace TheIdealShip.Patches;

// https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/SaveManagerPatch.cs
[HarmonyPatch]
public class SaveManagerPatch
{
    public static bool SaveToTISInfo = false;
    [HarmonyPatch(typeof(AmongUs.Data.Player.PlayerData), nameof(AmongUs.Data.Player.PlayerData.FileName), MethodType.Getter), HarmonyPostfix]
    public static void FileNamePatch_Postfix(ref string __result)
    {
        if (!SaveToTISInfo) return;
        __result += "_TIS";
    }

    [HarmonyPatch(typeof(AmongUs.Data.Legacy.LegacySaveManager), nameof(AmongUs.Data.Legacy.LegacySaveManager.GetPrefsName)), HarmonyPostfix]
    public static void LegacySaveManagerPatch_Postfix(ref string __result)
    {
        if (!SaveToTISInfo) return;
        __result += "_TIS";
    }
}