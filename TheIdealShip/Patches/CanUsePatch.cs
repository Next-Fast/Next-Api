using System.Collections.Generic;
using HarmonyLib;
using TheIdealShip.Utilities;
using UnityEngine;

namespace TheIdealShip.Patches;

[HarmonyPatch]
public static class CanUsePatch
{
    public static Dictionary<int, bool> CanUseMenDictionary = new Dictionary<int, bool>();
    public static bool AllowNoHostUse = false;
    
    [HarmonyPatch(typeof(OptionsConsole), nameof(OptionsConsole.CanUse)), HarmonyPrefix]
    public static bool OptionsConsoleCanUsePatch(OptionsConsole __instance, float __result, [HarmonyArgument(0)]GameData.PlayerInfo pc, 
        [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse)
    {
        if (!AllowNoHostUse) return true;
        float num = float.MaxValue;
        PlayerControl @object = pc.Object;
        couldUse = @object.CanMove;
        canUse = couldUse && CanUseMenDictionary[CachedPlayer.LocalPlayer.PlayerId];
        if (canUse)
        {
            num = Vector2.Distance(@object.GetTruePosition(), __instance.transform.position);
            canUse &= (num <= __instance.UsableDistance);
        }

        __result = num;
        return false;
    }
}