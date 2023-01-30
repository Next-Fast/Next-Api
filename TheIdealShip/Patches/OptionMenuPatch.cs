using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using UnityEngine.UI;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Start))]
    class OptionMenuPatch
    {
        public static TabGroup TISTabButton;
//        public static GameObject TISTabContent;
        public static void Postfix(OptionsMenuBehaviour __instance)
        {
            var tabs = new List<TabGroup>(__instance.Tabs.ToArray());
            if (tabs.Count() < 4) return;
          /*   if (TISTabContent == null)
            {
                TISTabContent = GameObject.Instantiate(tabs[2].Content, null);
                TISTabContent.name = "TISTabContent";
                TISTabContent.transform.SetParent(__instance.transform);
                TISTabContent.transform.localScale = new Vector3(1f, 1f, 1f);
                for (var i = 0; i < TISTabContent.transform.GetChildCount(); i++ )
                {
                    var r = TISTabContent.transform.GetChild(i);
                    if (r.name != "Languages")
                    {
                        GameObject.Destroy(r.gameObject);
                    }
                    if (r.name == "Languages")
                    {
                        r.name = "DownloadS";
                        r.transform.SetParent(TISTabContent.transform);
                    }
                }
                //FindChild("Languages")
                 var languageTemplate = tabs[2].Content.transform.FindChild("Languages").gameObject;
                var DownloadS = TISTabContent.transform.GetChild(0);
                var title = DownloadS.transform.FindChild("TitleText_TMP").gameObject;
                title.GetComponent<TextTranslatorTMP>().enabled = false;
                title.GetComponent<TMPro.TMP_Text>().text = "下载源";
                var bt = DownloadS.transform.GetChild(1).gameObject;
                bt.GetComponent<TextTranslatorTMP>().enabled = false;
                bt.GetComponent<TMPro.TMP_Text>().text = "";
                bt.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                bt.GetComponent<PassiveButton>().OnClick.AddListener((UnityEngine.Events.UnityAction)(() =>
            {
                TISTabContent.transform.GetChild(1).gameObject.SetActive(true);
            })); */
               /*  for (var i = 3; i < TISTabContent.transform.GetChild(5).GetChildCount(); i++)
                {
                    var r = TISTabContent.transform.GetChild(5).GetChild(i);
                    GameObject.Destroy(r.gameObject);
                }
            } */

            if (TISTabButton == null)
            {
                TISTabButton = GameObject.Instantiate(tabs[0],tabs[0].transform.parent);
                TISTabButton.name = "TheIdealShipTab";
//                TISTabButton.Content = TISTabContent;
                TISTabButton.Content = null;
                TISTabButton.transform.localPosition += new Vector3(4f,0,0);
                var text = TISTabButton.transform.FindChild("Text_TMP").gameObject;
                text.GetComponent<TextTranslatorTMP>().enabled = false;
                text.GetComponent<TMPro.TMP_Text>().text = "TIS";
                PassiveButton passiveButton = TISTabButton.GetComponent<PassiveButton>();
                passiveButton.OnClick = new Button.ButtonClickedEvent();
                passiveButton.OnClick.AddListener((UnityEngine.Events.UnityAction)(() =>
            {
                __instance.OpenTabGroup(4);

                passiveButton.OnMouseOver.Invoke();
            }));
                tabs.Add(TISTabButton);
            }
            float x = -2.25f;
            for (var i = 0; i < tabs.Count - 1; i++)
            {
                tabs[i].transform.localPosition = new Vector3( x + (1.5f * i), 0, 0);
            }
            for (var i = 0; i < tabs.Count; i++)
            {
                tabs[i].transform.localScale = new Vector3(0.9f, 1, 1);
            }
            __instance.Tabs = new Il2CppReferenceArray<TabGroup>(tabs.ToArray());
        }
    }
}