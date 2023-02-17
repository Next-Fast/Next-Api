// using HarmonyLib;
// using UnityEngine;
//
// namespace TheIdealShip.Patches;
//
// [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
// public class GameOptionsMenuUIPacth
// {
//     public static void Postfix()
//     {
//         var ChatAndSettingsButtonBackground = GameObject.Find("ChatAndSettingsButtonBackground");
//         if (ChatAndSettingsButtonBackground != null) ChatAndSettingsButtonBackground.SetActive(false);
//
//         var Close = GameObject.Find("CloseButton/CloseButtonBackground");
//         if (Close != null) Close.SetActive(false);
//         
//         var Header = GameObject.Find("Header");
//         SpriteRenderer HeaderSprite = Header.GetComponent<SpriteRenderer>();
//         HeaderSprite.sprite = null;
//         // if (Header != null) Header.SetActive(false);
//
//         var BaseGlass = Header.transform.FindChild("baseGlass").gameObject;
//         BaseGlass.SetActive(false);
//
//         var GameSettings = GameObject.Find("Game Settings");
//
//         var Panel = GameObject.Find("Game Settings/BackPanel");
//         if (Panel != null) Panel.SetActive(false);
//
//         var Reset = GameObject.Find("SliderInner/ResetToDefault");
//         if (Reset != null) Reset.SetActive(false);
//
//         var SliderInner = GameObject.Find("GameGroup/SliderInner");
//         for (int i = 0; i < SliderInner.transform.childCount; i++)
//         {
//             var option = SliderInner.transform.GetChild(i);
//             for (int j = 0; j < option.transform.childCount; j++)
//             {
//                 var Optionobject = option.transform.GetChild(j);
//                 if (Optionobject.name == "Background") 
//                     Optionobject.gameObject.SetActive(false);
//                 if (Optionobject.name == "CheckBox")
//                     Optionobject.gameObject.GetComponent<SpriteRenderer>().sprite = null;
//             }
//         }
//     }
// }