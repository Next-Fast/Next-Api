using HarmonyLib;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.Open))]
    public static class RegionMenuOpenPatch
    {
        public static int ye = 1;
        public static int maxye;
        private static GameObject shangButton;
        private static GameObject xiaButton;
        private static Vector3 pos = new Vector3(3f, 2.5f, -100f);
        private static ServerManager serverManager = DestroyableSingleton<ServerManager>.Instance;
        public static void Postfix(RegionMenu __instance)
        {
            if (xiaButton == null || xiaButton.gameObject == null)
            {
                GameObject tf = GameObject.Find("NormalMenu/BackButton");

                xiaButton = UnityEngine.Object.Instantiate(tf, __instance.transform);
                xiaButton.name = "xiaButton";
                xiaButton.transform.position = pos - new Vector3(-0.6f, 3f, 0f);

                var xiaButtontext = xiaButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                PassiveButton xiaButtonPassiveButton = xiaButton.GetComponent<PassiveButton>();
                SpriteRenderer xiaButtonButtonSprite = xiaButton.GetComponent<SpriteRenderer>();
                xiaButtonPassiveButton.OnClick = new();
                xiaButtonPassiveButton.OnClick.AddListener((UnityAction)ClearAllButtonVoid);
                xiaButtontext.SetText("下一页");
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => xiaButtontext.SetText("下一页"))));
                xiaButton.gameObject.SetActive(!(serverManager.AvailableRegions.Count <= 6));

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
                GameObject tf = GameObject.Find("NormalMenu/BackButton");

                shangButton = UnityEngine.Object.Instantiate(tf, __instance.transform);
                shangButton.name = "shangButton";
                shangButton.transform.position = pos - new Vector3(0.6f, 3f, 0f);

                var shangButtontext = shangButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                PassiveButton shangButtonPassiveButton = shangButton.GetComponent<PassiveButton>();
                SpriteRenderer shangButtonButtonSprite = shangButton.GetComponent<SpriteRenderer>();
                shangButtonPassiveButton.OnClick = new();
                shangButtonPassiveButton.OnClick.AddListener((UnityAction)ClearAllButtonVoid);
                shangButtontext.SetText("上一页");
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => shangButtontext.SetText("上一页"))));
                xiaButton.gameObject.SetActive(!(serverManager.AvailableRegions.Count <= 6));

                void ClearAllButtonVoid()
                {
                    if (ye > 1)
                    {
                        ye--;
                        Menu.shuaXing(__instance);
                    }
                }
            }
            if (xiaButton.transform.position != pos - new Vector3(-0.6f, 3f, 0f)) xiaButton.transform.position = pos - new Vector3(-0.6f, 3f, 0f);

            if (shangButton.transform.position != pos - new Vector3(0.6f, 3f, 0f)) shangButton.transform.position = pos - new Vector3(0.6f, 3f, 0f);
        }

        public static void autoAddServer()
        {
            IRegionInfo[] regionInfos = new IRegionInfo[]
            {
                createHttp("au-sh.pafyx.top", "梦服上海(新)", 22000, false),
                createHttp("au-as.duikbo.at", "Modded Asia (MAS)", 443, true),
                createHttp("www.aumods.xyz", "Modded NA (MNA)", 443, true),
                createHttp("au-eu.duikbo.at", "Modded EU (MEU)", 443, true),
            };

            if (!TheIdealShip.TheIdealShipPlugin.isChinese) regionInfos.ToList().RemoveAt(0);

            foreach (var r in regionInfos)
            {
                if (serverManager.AvailableRegions.Contains(r)) continue;
                serverManager.AddOrUpdateRegion(r);
            }
        }

        public static IRegionInfo createHttp(string ip, string name, ushort port, bool ishttps)
        {
            string serverIp = ishttps ? "https://" : "http://" + ip;
            ServerInfo serverInfo = new ServerInfo(name, serverIp, port, false);
            ServerInfo[] ServerInfo = new ServerInfo[] { serverInfo };
            return new StaticHttpRegionInfo(name, StringNames.NoTranslation, ip, ServerInfo).CastFast<IRegionInfo>();
        }
    }

    [HarmonyPatch]
    public static class Menu
    {
        private static ServerManager serverManager = DestroyableSingleton<ServerManager>.Instance;

        [HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.OnEnable)), HarmonyPrefix]
        public static bool RegionMenuOnEnablePatch(RegionMenu __instance)
        {
            if (serverManager.AvailableRegions.Count <= 3) return true;
            CreateRegionMenu(__instance);
            return false;
        }

        private static void CreateRegionMenu(RegionMenu __instance)
        {
            CreateServerOption(__instance);
            ControllerManager.Instance.OpenOverlayMenu(__instance.name, __instance.BackButton, __instance.defaultButtonSelected, __instance.controllerSelectable, false);
        }

        private static void CreateServerOption(RegionMenu __instance)
        {
            RegionMenuOpenPatch.maxye = (serverManager.AvailableRegions.Count / 3) + (serverManager.AvailableRegions.Count % 3 == 0 ? 0 : 1);
            IRegionInfo[] regionInfos = new IRegionInfo[1];
            if (serverManager.AvailableRegions.Count < 6)
            {
                regionInfos = serverManager.AvailableRegions;
            }
            else
            {
                updateyer();
            }

            void updateyer()
            {
                List<IRegionInfo> rlist = new List<IRegionInfo>();
                for (int i = 0; i < 3; i++)
                {
                    int s = RegionMenuOpenPatch.ye * 3 - i;
                    if (s <= serverManager.AvailableRegions.Count)
                    {
                        rlist.Add(serverManager.AvailableRegions[s - 1]);
                    }
                    regionInfos = rlist.ToArray();
                }
            }

            __instance.controllerSelectable.Clear();
            int num = 0;
            foreach (IRegionInfo regionInfo in regionInfos)
            {
                IRegionInfo region = regionInfo;
                ServerListButton serverListButton = __instance.ButtonPool.Get<ServerListButton>();
                serverListButton.transform.localPosition = new Vector3(0f, 1.5f - 0.5f * (float)num, 0f);
                serverListButton.SetTextTranslationId(regionInfo.TranslateName, regionInfo.Name);
                serverListButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(regionInfo.TranslateName, regionInfo.Name);
                serverListButton.Text.ForceMeshUpdate(false, false);
                serverListButton.Button.OnClick.RemoveAllListeners();
                serverListButton.Button.OnClick.AddListener((UnityAction)(() =>
                {
                    __instance.ChooseOption(region);
                }));
                serverListButton.SetSelected(serverManager.CurrentRegion.Name == regionInfo.Name);
                if (DestroyableSingleton<ServerManager>.Instance.CurrentRegion.Equals(regionInfo))
                {
                    __instance.defaultButtonSelected = serverListButton.Button;
                }
                __instance.controllerSelectable.Add(serverListButton.Button);
                num++;
            }
            if (__instance.defaultButtonSelected == null && __instance.controllerSelectable.Count > 0)
            {
                __instance.defaultButtonSelected = __instance.controllerSelectable[0];
            }
        }

        public static void shuaXing(RegionMenu __instance)
        {
            __instance.ButtonPool.ReclaimAll();
            CreateServerOption(__instance);
        }
    }
}