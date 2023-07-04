using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Start))]
    class OptionMenuPatch
    {
        public static TabGroup TISTabButton;
        public static GameObject DownloadS;
        public static GameObject TISTabContent;
        public static void Postfix(OptionsMenuBehaviour __instance)
        {
            var tabs = new List<TabGroup>(__instance.Tabs.ToArray());
            if (tabs.Count() < 4) return;
            if (TISTabContent == null)
            {
                TISTabContent = new GameObject("TISTabContent");
                TISTabContent.transform.SetParent(__instance.transform);
                TISTabContent.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            /*if (DownloadS == null)
            {

                DownloadS = new GameObject(nameof(DownloadS));
                DownloadS.transform.SetParent(TISTabContent.transform);
                Il2CppInterop.Runtime.Injection.ClassInjector.RegisterTypeInIl2Cpp<Dropdown>();
                var dr = DownloadS.AddComponent<Dropdown>();

                Sprite sprite = tabs[0].transform.FindChild("Background").GetComponent<SpriteRenderer>().sprite;
                Il2CppSystem.Collections.Generic.List<Dropdown.OptionData> optionDatas = new();
                List<string> OptionText = new();
                VersionManager.URLs.Do(n => OptionText.Add(n.Item2));
                OptionText.Add("Auto");
                foreach (var text in OptionText)
                {
                    Dropdown.OptionData optionData = new();
                    optionData.text = text;
                    optionDatas.Add(optionData);
                }

                dr.ClearOptions();
                dr.AddOptions(optionDatas);
            }*/

            if (TISTabButton == null)
            {
                TISTabButton = GameObject.Instantiate(tabs[0],tabs[0].transform.parent);
                TISTabButton.name = "TheIdealShipTab";
                TISTabButton.Content = TISTabContent;
                TISTabButton.transform.localPosition += new Vector3(4f,0,0);
                var text = TISTabButton.transform.FindChild("Text_TMP").gameObject;
                text.DestroyTranslator();
                text.GetComponent<TMPro.TMP_Text>().text = "TIS";
                PassiveButton passiveButton = TISTabButton.GetComponent<PassiveButton>();
                passiveButton.OnClick = new Button.ButtonClickedEvent();
                passiveButton.OnClick.AddListener((Action)(() =>
            {
                for (var i = 0; i < tabs.Count; i++ )
                {
                    if(tabs[i].Content == TISTabContent)
                    {
                        __instance.OpenTabGroup(i);
                    }
                }

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

        [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Update)), HarmonyPostfix]
        public static void UpdatePatch()
        {
            DownloadS.active = true;
            DownloadS.GetComponent<Dropdown>().Show();
        }
    }
}