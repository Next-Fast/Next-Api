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
        public static GameObject TISTabContent;
        public static void Postfix(OptionsMenuBehaviour __instance)
        {
            var tabs = new List<TabGroup>(__instance.Tabs.ToArray());
            if (TISTabButton == null)
            {
                TISTabButton = GameObject.Instantiate(tabs[0],tabs[0].transform.parent);
                TISTabButton.name = "TheIdealShipTab";
                /* var ttext = TISTabButton.transform.GetChild(0).gameObject;
                ttext.GetComponent<TMPro.TMP_Text>().text = "下载源"; */
                TISTabButton.Content = TISTabContent = null;
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