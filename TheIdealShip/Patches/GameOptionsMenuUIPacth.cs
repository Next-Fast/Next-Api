using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

namespace TheIdealShip.Patches;

[HarmonyPatch]
public static class GameOptionsMenuUIPacth
{
/*     [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start)), HarmonyPostfix]
    public static void Start_Postfix()
    {
        var ChatAndSettingsButtonBackground = GameObject.Find("ChatAndSettingsButtonBackground");
        if (ChatAndSettingsButtonBackground != null) ChatAndSettingsButtonBackground.SetActive(false);

        var CloseBackground = GameObject.Find("CloseButton/CloseButtonBackground");
        if (CloseBackground != null) CloseBackground.SetActive(false);

        var Header = GameObject.Find("Header");
        SpriteRenderer HeaderSprite = Header.GetComponent<SpriteRenderer>();
        HeaderSprite.sprite = null;
        // if (Header != null) Header.SetActive(false);

        var BaseGlass = Header.transform.FindChild("baseGlass").gameObject;
        BaseGlass.SetActive(false);

        foreach
        (
            var gs in
        new List<string>()
        {
            "TISSettings",
            "ImpostorSettings" ,
            "NeutralSettings",
            "CrewmateSettings",
            "ModifierSettings"
        }
        )
        {
            var GameSettings = GameObject.Find(gs);
            if (GameSettings != null) GameSettings.transform.position = new Vector3(GameSettings.transform.position.x, 0.5f, GameSettings.transform.position.z);
        }

        var Panel = GameObject.Find("Game Settings/BackPanel");
        if (Panel != null) Panel.SetActive(false);

        var Reset = GameObject.Find("SliderInner/ResetToDefault");
        if (Reset != null) Reset.SetActive(false);

        var Text = GameObject.Find("GameGroup/Text");
        Text.transform.localPosition += new Vector3(1.6f, 0.9f, Text.transform.localPosition.z);

        var SliderInner = GameObject.Find("GameGroup/SliderInner");
        for (int i = 0; i < SliderInner.transform.childCount; i++)
        {
            var option = SliderInner.transform.GetChild(i);
            for (int j = 0; j < option.transform.childCount; j++)
            {
                var Optionobject = option.transform.GetChild(j);
                if (Optionobject.name == "Background")
                    Optionobject.gameObject.SetActive(false);
                // if (Optionobject.name == "CheckBox")
                //     Optionobject.gameObject.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    } */

/*     [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start)), HarmonyPostfix]
    public static void setting_Postfix(GameSettingMenu __instance)
    {
        __instance.transform.localPosition = new Vector3(__instance..transform.localPosition.x, 0.5f, __instance.transform.localPosition.z);
    } */
}