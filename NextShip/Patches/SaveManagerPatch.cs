using AmongUs.Data.Legacy;
using AmongUs.Data.Player;
using HarmonyLib;

namespace NextShip.Patches;

// https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/SaveManagerPatch.cs
[HarmonyPatch]
public class SaveManagerPatch
{
    public static bool SaveToTISInfo = false;

    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.FileName), MethodType.Getter)]
    [HarmonyPostfix]
    public static void FileNamePatch_Postfix(ref string __result)
    {
        if (!SaveToTISInfo) return;
        __result += "_TIS";
    }

    [HarmonyPatch(typeof(LegacySaveManager), nameof(LegacySaveManager.GetPrefsName))]
    [HarmonyPostfix]
    public static void LegacySaveManagerPatch_Postfix(ref string __result)
    {
        if (!SaveToTISInfo) return;
        __result += "_TIS";
    }
}