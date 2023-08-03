using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace NextShip.Patches;

[HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.Open))]
public static class RegionMenuOpenPatch
{
    public static int ye = 1;
    public static int maxye;
    private static GameObject shangButton;
    private static GameObject xiaButton;
    private static readonly Vector3 pos = new(3f, 2.5f, -100f);

    public static void Postfix(RegionMenu __instance)
    {
        if (xiaButton == null || xiaButton.gameObject == null)
        {
            var tf = GameObject.Find("NormalMenu/BackButton");
            if (!tf) return;

            xiaButton = Object.Instantiate(tf, __instance.transform);
            xiaButton.name = "xiaButton";
            xiaButton.transform.position = pos - new Vector3(-0.6f, 3f, 0f);

            var xiaButtontext = xiaButton.transform.GetChild(0).GetComponent<TMP_Text>();
            var xiaButtonPassiveButton = xiaButton.GetComponent<PassiveButton>();
            var xiaButtonButtonSprite = xiaButton.GetComponent<SpriteRenderer>();
            xiaButtonPassiveButton.OnClick = new Button.ButtonClickedEvent();
            xiaButtonPassiveButton.OnClick.AddListener((UnityAction)ClearAllButtonVoid);
            xiaButtontext.SetText("下一页");
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>(p => xiaButtontext.SetText("下一页"))));
            xiaButton.gameObject.SetActive(!(Main.serverManager.AvailableRegions.Count <= 6));

            void ClearAllButtonVoid()
            {
                if (ye < maxye)
                {
                    ye++;
                    Menu.shuaXing(__instance);
                }
            }
        }

        if (shangButton == null || shangButton.gameObject == null)
        {
            var tf = GameObject.Find("NormalMenu/BackButton");

            shangButton = Object.Instantiate(tf, __instance.transform);
            shangButton.name = "shangButton";
            shangButton.transform.position = pos - new Vector3(0.6f, 3f, 0f);

            var shangButtontext = shangButton.transform.GetChild(0).GetComponent<TMP_Text>();
            var shangButtonPassiveButton = shangButton.GetComponent<PassiveButton>();
            var shangButtonButtonSprite = shangButton.GetComponent<SpriteRenderer>();
            shangButtonPassiveButton.OnClick = new Button.ButtonClickedEvent();
            shangButtonPassiveButton.OnClick.AddListener((UnityAction)ClearAllButtonVoid);
            shangButtontext.SetText("上一页");
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>(p => shangButtontext.SetText("上一页"))));
            xiaButton.gameObject.SetActive(!(Main.serverManager.AvailableRegions.Count <= 6));

            void ClearAllButtonVoid()
            {
                if (ye > 1)
                {
                    ye--;
                    Menu.shuaXing(__instance);
                }
            }
        }

        if (xiaButton.transform.position != pos - new Vector3(-0.6f, 3f, 0f))
            xiaButton.transform.position = pos - new Vector3(-0.6f, 3f, 0f);

        if (shangButton.transform.position != pos - new Vector3(0.6f, 3f, 0f))
            shangButton.transform.position = pos - new Vector3(0.6f, 3f, 0f);
    }
}

[HarmonyPatch]
public static class Menu
{
    private static readonly ServerManager serverManager = DestroyableSingleton<ServerManager>.Instance;

    [HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.OnEnable))]
    [HarmonyPrefix]
    public static bool RegionMenuOnEnablePatch(RegionMenu __instance)
    {
        if (serverManager.AvailableRegions.Count <= 3) return true;
        CreateRegionMenu(__instance);
        return false;
    }

    private static void CreateRegionMenu(RegionMenu __instance)
    {
        CreateServerOption(__instance);
        ControllerManager.Instance.OpenOverlayMenu(__instance.name, __instance.BackButton,
            __instance.defaultButtonSelected, __instance.controllerSelectable);
    }

    private static void CreateServerOption(RegionMenu __instance)
    {
        RegionMenuOpenPatch.maxye = serverManager.AvailableRegions.Count / 3 +
                                    (serverManager.AvailableRegions.Count % 3 == 0 ? 0 : 1);
        var regionInfos = new IRegionInfo[1];
        if (serverManager.AvailableRegions.Count < 6)
            regionInfos = serverManager.AvailableRegions;
        else
            updateyer();

        void updateyer()
        {
            var rlist = new List<IRegionInfo>();
            for (var i = 0; i < 3; i++)
            {
                var s = RegionMenuOpenPatch.ye * 3 - i;
                if (s <= serverManager.AvailableRegions.Count) rlist.Add(serverManager.AvailableRegions[s - 1]);
                regionInfos = rlist.ToArray();
            }
        }

        __instance.controllerSelectable.Clear();
        List<UiElement> List = new();
        var num = 0;
        foreach (var regionInfo in regionInfos)
        {
            var region = regionInfo;
            var serverListButton = __instance.ButtonPool.Get<ServerListButton>();
            serverListButton.transform.localPosition = new Vector3(0f, 1.5f - 0.5f * num, 0f);
            serverListButton.SetTextTranslationId(regionInfo.TranslateName, regionInfo.Name);
            serverListButton.Text.text =
                DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(regionInfo.TranslateName,
                    regionInfo.Name);
            serverListButton.Text.ForceMeshUpdate();
            serverListButton.Button.OnClick.RemoveAllListeners();
            serverListButton.Button.OnClick.AddListener((Action)(() => { __instance.ChooseOption(region); }));
            serverListButton.SetSelected(serverManager.CurrentRegion.Name == regionInfo.Name);
            if (DestroyableSingleton<ServerManager>.Instance.CurrentRegion.Equals(regionInfo))
                __instance.defaultButtonSelected = serverListButton.Button;
            List.Add(serverListButton.Button);
            num++;
        }

        if (__instance.defaultButtonSelected == null && __instance.controllerSelectable.Count > 0)
            __instance.defaultButtonSelected = List[0];

        List.Do(n => __instance.controllerSelectable.Add(n));
    }

    public static void shuaXing(RegionMenu __instance)
    {
        __instance.ButtonPool.ReclaimAll();
        CreateServerOption(__instance);
    }
}