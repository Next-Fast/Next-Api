using System.Collections.Generic;
using HarmonyLib;
using NextShip.Utilities;
using UnityEngine;
using NextShip.UI.Components;
using Object = UnityEngine.Object;

namespace NextShip.Options.Patches;

[HarmonyPatch]
public class OptionsConsolePatch
{
    public static Dictionary<int, bool> CanUseMenDictionary = new();
    public static bool AllowNoHostUse = false;
    public static bool IsNextMenu;
    public static NextOptionMenu NextOptionMenu;

    private static readonly GameObject NextMenuParent = new ("NextMenuParent");

    [HarmonyPatch(typeof(OptionsConsole), nameof(OptionsConsole.Use))]
    [HarmonyPrefix]
    public static bool OptionsConsoleUsePatch(OptionsConsole __instance)
    {
        /*if (!AllowNoHostUse) return true;*/
        
        var @object = PlayerControl.LocalPlayer;
        var couldUse = @object.CanMove;
        var canUse =
            couldUse
            && (!CanUseMenDictionary.TryGetValue(CachedPlayer.LocalPlayer.PlayerId, out var canUsed) || canUsed)
            && Vector2.Distance(@object.GetTruePosition(), __instance.transform.position) <= __instance.UsableDistance;

        if (!canUse) return false;
        
        NextOptionMenu ??= new NextOptionMenu(__instance, NextMenuParent);
        NextOptionMenu.__instance = __instance;

        if (!NextOptionMenu.Initd) NextOptionMenu.Init();
        
        if (NextOptionMenu.Initd) 
            OpenNextOptionMenu(__instance);
        else
            OpenVanillaOptionMenu(__instance);

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


    private static void OpenNextOptionMenu(OptionsConsole __instance)
    {      
        if (!Camera.main) return;
        PlayerControl.LocalPlayer.NetTransform.Halt();
        GameObject optionMenu;
        if (!(optionMenu = GameObject.Find("NextMenuParent(Clone)"))) 
            optionMenu = Object.Instantiate(NextMenuParent, Camera.main.transform, false);
        
        optionMenu.transform.localPosition = __instance.CustomPosition;
        FastDestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, optionMenu, null);
        IsNextMenu = true;
    }
}