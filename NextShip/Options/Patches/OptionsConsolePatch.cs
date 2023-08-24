using System.Collections.Generic;
using HarmonyLib;
using NextShip.Utilities;
using UnityEngine;
using AmongUs.QuickChat;
using TMPro;

namespace NextShip.Options.Patches;

[HarmonyPatch]
public class OptionsConsolePatch
{
    public static Dictionary<int, bool> CanUseMenDictionary = new();
    public static bool AllowNoHostUse = false;
    public static bool IsNextMenu;

    public static GameObject NextMenuParent = new ("NextMenuParent");
    public static GameObject NextMenu;

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

        var Init = InitNextOptionMenu(__instance);

        if (!canUse) return false;
        
        if (Init) OpenNextOptionMenu(__instance);
        
        if (!Init) OpenVanillaOptionMenu(__instance);

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
        if (!Camera.main.transform) return false;
        if (NextMenuParent.transform.parent) return true;
        if (NextMenu) return true;
        /*var _AbstractQuickChatMenuPhrasesPageButton = FastDestroyableSingleton<AbstractQuickChatMenuPhrasesPageButton>.Instance;*/

        var tint =Object.Instantiate(__instance.MenuPrefab.transform.Find("Tint"), NextMenuParent.transform);
        var background =Object.Instantiate(__instance.MenuPrefab.transform.Find("Background"), NextMenuParent.transform);

        CreateButton("Abab", "abab", "button");
            
        NextMenu = new GameObject("NextMenu");
        NextMenu.transform.SetParent(NextMenuParent.transform);
        NextMenuParent.SetActive(false);
        return true;

        GameObject CreateButton(string Title, string text, string name)
        {
            GameObject button = new GameObject(name);
            button.layer = tint.gameObject.layer;
            button.transform.SetParent(NextMenuParent.transform);
            button.CreatePassiveButton();
            var backGround = new GameObject("BackGround");
            backGround.transform.SetParent(button.transform);
            var backGroundSprite = backGround.AddComponent<SpriteRenderer>();
            backGroundSprite.sprite = ObjetUtils.Find<Sprite>("button");
            backGroundSprite.drawMode = SpriteDrawMode.Sliced;
            button.AddComponent<BoxCollider2D>().size = backGroundSprite.size;

            var titleTextGameObject = new GameObject("TitleText");
            titleTextGameObject.transform.SetParent(button.transform);
            titleTextGameObject.AddComponent<TextMeshPro>().text = Title;
            
            /*var SubTextGameObject = Object.Instantiate(TitleGameObject);
            SubTextGameObject.name = "SubText";*/
            
            return button;
        }
    }

    public static void OpenNextOptionMenu(OptionsConsole __instance)
    {      
        if (!Camera.main) return;
        PlayerControl.LocalPlayer.NetTransform.Halt();
        var optionMenu = Object.Instantiate(NextMenuParent, Camera.main.transform, false);
        optionMenu.transform.localPosition = __instance.CustomPosition;
        FastDestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, optionMenu, null);
        IsNextMenu = true;
    }
}