using System.Collections.Generic;
using HarmonyLib;
using NextShip.Utilities;
using UnityEngine;
using AmongUs.QuickChat;

namespace NextShip.Options.Patches;

[HarmonyPatch]
public class OptionsConsolePatch
{
    public static Dictionary<int, bool> CanUseMenDictionary = new();
    public static bool AllowNoHostUse = false;
    public static bool IsNextMenu;

    public static GameObject NextMenuParent = new GameObject();

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

        if (canUse && InitNextOptionMenu(__instance)) OpenNextOptionMenu(__instance);
        
        if (canUse) OpenVanillaOptionMenu(__instance);

        return false;
    }

    public static void OpenVanillaOptionMenu(OptionsConsole __instance)
    {
        if (!Camera.main) return;
        PlayerControl.LocalPlayer.NetTransform.Halt();
        var optionMenu = Object.Instantiate(__instance.MenuPrefab, Camera.main.transform, false);
        optionMenu.transform.localPosition = __instance.CustomPosition;
        FastDestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, optionMenu.gameObject, null);
        IsNextMenu = false;
    }

    public static bool InitNextOptionMenu(OptionsConsole __instance)
    {
        if (!Camera.main) return false;
        var VanillaMenu = __instance.MenuPrefab;
        var _AbstractQuickChatMenuPhrasesPageButton = FastDestroyableSingleton<AbstractQuickChatMenuPhrasesPageButton>.Instance;
        
        
        NextMenuParent.transform.SetParent(Camera.main.transform);

        Object.Instantiate(__instance.MenuPrefab.transform.Find("Tint"), NextMenuParent.transform);
        Object.Instantiate(__instance.MenuPrefab.transform.Find("Background"), NextMenuParent.transform);
            
        GameObject NextMenu = new GameObject();
        NextMenu.transform.SetParent(NextMenuParent.transform);
        return false;
    }

    public static void OpenNextOptionMenu(OptionsConsole __instance)
    {      
        if (!Camera.main) return;
        PlayerControl.LocalPlayer.NetTransform.Halt();
        FastDestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, NextMenuParent, null);
        IsNextMenu = true;
    }
}