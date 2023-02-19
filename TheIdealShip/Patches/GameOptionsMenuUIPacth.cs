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
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start)), HarmonyPostfix]
    public static void Start_Postfix()
    {
        var ChatAndSettingsButtonBackground = GameObject.Find("ChatAndSettingsButtonBackground");
        if (ChatAndSettingsButtonBackground != null) ChatAndSettingsButtonBackground.SetActive(false);

        // var Close = GameObject.Find("CloseButton");
        // Close.GetComponent<SpriteRenderer>().sprite = Helpers.LoadSpriteFromResources("TheIdealShip.Resources.closeButton.png",100f);
        // //
        // var Chat = GameObject.Find("ChatButton");
        // Chat.GetComponent<SpriteRenderer>().sprite = Helpers.LoadSpriteFromResources("TheIdealShip.Resources.chatIcon.png",100f);
        
        var CloseBackground = GameObject.Find("CloseButton/CloseButtonBackground");
        if (CloseBackground != null) CloseBackground.SetActive(false);
        
        var Header = GameObject.Find("Header");
        SpriteRenderer HeaderSprite = Header.GetComponent<SpriteRenderer>();
        HeaderSprite.sprite = null;
        // if (Header != null) Header.SetActive(false);

        var BaseGlass = Header.transform.FindChild("baseGlass").gameObject;
        BaseGlass.SetActive(false);
        
        // var GameSettings = GameObject.Find("Game Settings");
        // if (GameSettings != null) GameSettings.transform.position += Vector3.up * 1f;

        var Panel = GameObject.Find("Game Settings/BackPanel");
        if (Panel != null) Panel.SetActive(false);

        var Reset = GameObject.Find("SliderInner/ResetToDefault");
        if (Reset != null) Reset.SetActive(false);
        
        // var Text = GameObject.Find("GameGroup/Text");
        // Text.transform.position += Vector3.right  * 1.5f;

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
    }
}