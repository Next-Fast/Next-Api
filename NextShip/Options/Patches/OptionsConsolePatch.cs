using System.Collections.Generic;
using HarmonyLib;
using NextShip.Utilities;
using UnityEngine;

namespace NextShip.Options.Patches;

[HarmonyPatch]
public class OptionsConsolePatch
{
    public static Dictionary<int, bool> CanUseMenDictionary = new();
    public static bool AllowNoHostUse = false;

    [HarmonyPatch(typeof(OptionsConsole), nameof(OptionsConsole.Use))]
    [HarmonyPrefix]
    public static bool OptionsConsoleUsePatch(OptionsConsole __instance)
    {
        if (!AllowNoHostUse) return true;
        var @object = PlayerControl.LocalPlayer;
        var couldUse = @object.CanMove;
        var canUse = 
            couldUse 
            && CanUseMenDictionary[CachedPlayer.LocalPlayer.PlayerId] 
            && Vector2.Distance(@object.GetTruePosition(), __instance.transform.position) <= __instance.UsableDistance;
        
        if (canUse)
        {
            OpenVanillaOptionMenu(__instance);
        }

        return false;
    }

    public static void OpenVanillaOptionMenu(OptionsConsole __instance)
    {
        if (Camera.main == null) return;
        PlayerControl.LocalPlayer.NetTransform.Halt();
        GameObject optionMenu = Object.Instantiate(__instance.MenuPrefab, Camera.main.transform, false);
        optionMenu.transform.localPosition = __instance.CustomPosition;
        FastDestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, optionMenu.gameObject, null);
    }
}