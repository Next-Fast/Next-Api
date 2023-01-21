
// Adapted from https://github.com/MoltenMods/Unify
/*
MIT License
Copyright (c) 2021 Daemon
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System;
using TheIdealShip.Utilities;
using UnityEngine.Events;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.Open))]
    public static class RegionMenuOpenPatch
    {
        private static TextBoxTMP ipField;
        private static TextBoxTMP portField;
        private static GameObject isHttpsButton;
        private static Vector3 pos = new Vector3(3f, 2f, -100f);

        public static void Postfix(RegionMenu __instance)
        {
            if (!__instance.TryCast<RegionMenu>()) return;
            bool isCustomRegion = FastDestroyableSingleton<ServerManager>.Instance.CurrentRegion.Name == "Custom";
            if (!isCustomRegion)
            {
                if (ipField != null && ipField.gameObject != null)
                {
                    ipField.gameObject.SetActive(false);

                }
                if (portField != null && portField.gameObject != null)
                {
                    portField.gameObject.SetActive(false);
                }
                if (isHttpsButton != null)
                {
                    isHttpsButton.gameObject.SetActive(false);
                }
            }
            else
            {
                if (ipField != null && ipField.gameObject != null)
                {
                    ipField.gameObject.SetActive(true);

                }
                if (portField != null && portField.gameObject != null)
                {
                    portField.gameObject.SetActive(true);
                }
                if (isHttpsButton != null)
                {
                    isHttpsButton.gameObject.SetActive(true);
                }
            }
            var template = FastDestroyableSingleton<JoinGameButton>.Instance;
            var joinGameButtons = GameObject.FindObjectsOfType<JoinGameButton>();
            foreach (var t in joinGameButtons)
            {  // The correct button has a background, the other 2 dont
                if (t.GameIdText != null && t.GameIdText.Background != null)
                {
                    template = t;
                    break;
                }
            }
            if (template == null || template.GameIdText == null) return;

            if (ipField == null || ipField.gameObject == null)
            {
                ipField = UnityEngine.Object.Instantiate(template.GameIdText, __instance.transform);
                ipField.gameObject.name = "IpTextBox";
                var arrow = ipField.transform.FindChild("arrowEnter");
                if (arrow == null || arrow.gameObject == null) return;
                UnityEngine.Object.DestroyImmediate(arrow.gameObject);

                ipField.transform.localPosition = pos - new Vector3(0f, 1f, 0f);
                ipField.characterLimit = 30;
                ipField.AllowSymbols = true;
                ipField.ForceUppercase = false;
                ipField.SetText(TheIdealShipPlugin.CustomIp.Value.ToString());
                __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) =>
                {
                    ipField.outputText.SetText(TheIdealShipPlugin.CustomIp.Value.ToString());
                    ipField.SetText(TheIdealShipPlugin.CustomIp.Value.ToString());
                })));

                ipField.ClearOnFocus = false;
                ipField.OnEnter = ipField.OnChange = new Button.ButtonClickedEvent();
                ipField.OnFocusLost = new Button.ButtonClickedEvent();
                ipField.OnChange.AddListener((UnityAction)onEnterOrIpChange);
                ipField.OnFocusLost.AddListener((UnityAction)onFocusLost);
                ipField.gameObject.SetActive(isCustomRegion);

                void onEnterOrIpChange()
                {
                    TheIdealShipPlugin.CustomIp.Value = ipField.text;
                }

                void onFocusLost()
                {
                    UpdateRegions();
                    __instance.ChooseOption(ServerManager.DefaultRegions[ServerManager.DefaultRegions.Length - 1]);
                }
            }
            if (portField == null || portField.gameObject == null)
            {

                portField = UnityEngine.Object.Instantiate(template.GameIdText, __instance.transform);
                portField.gameObject.name = "PortTextBox";
                var arrow = portField.transform.FindChild("arrowEnter");
                UnityEngine.Object.DestroyImmediate(arrow.gameObject);

                portField.transform.localPosition = pos - new Vector3(0f, 2f, 0f);
                portField.characterLimit = 5;
                portField.SetText(TheIdealShipPlugin.CustomPort.Value.ToString());
                __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) =>
                {
                    portField.outputText.SetText(TheIdealShipPlugin.CustomPort.Value.ToString());
                    portField.SetText(TheIdealShipPlugin.CustomPort.Value.ToString());
                })));


                portField.ClearOnFocus = false;
                portField.OnEnter = portField.OnChange = new Button.ButtonClickedEvent();
                portField.OnFocusLost = new Button.ButtonClickedEvent();
                portField.OnChange.AddListener((UnityAction)onEnterOrPortFieldChange);
                portField.OnFocusLost.AddListener((UnityAction)onFocusLost);
                portField.gameObject.SetActive(isCustomRegion);

                void onEnterOrPortFieldChange()
                {
                    ushort port = 0;
                    if (ushort.TryParse(portField.text, out port))
                    {
                        TheIdealShipPlugin.CustomPort.Value = port;
                        portField.outputText.color = Color.white;
                    }
                    else
                    {
                        portField.outputText.color = Color.red;
                    }
                }
                void onFocusLost()
                {
                    UpdateRegions();
                    __instance.ChooseOption(ServerManager.DefaultRegions[ServerManager.DefaultRegions.Length - 1]);
                }
            }

            if (isHttpsButton == null || isHttpsButton.gameObject == null)
            {
                GameObject tf = GameObject.Find("NormalMenu/BackButton");

                isHttpsButton = UnityEngine.Object.Instantiate(tf,__instance.transform);
                isHttpsButton.name = "isHttpsButton";
                isHttpsButton.transform.position = pos - new Vector3(0f, 3f, 0f);

                var text = isHttpsButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                PassiveButton isHttpsPassiveButton = isHttpsButton.GetComponent<PassiveButton>();
                SpriteRenderer isHttpsButtonSprite = isHttpsButton.GetComponent<SpriteRenderer>();
                isHttpsPassiveButton.OnClick = new();
                isHttpsPassiveButton.OnClick.AddListener((UnityAction)act);
                text.SetText("isHttps");
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => text.SetText("isHttps"))));
                isHttpsButton.gameObject.SetActive(isCustomRegion);

                void act()
                {
                    if (TheIdealShipPlugin.isHttps.Value)
                    {
                        TheIdealShipPlugin.isHttps.Value = false;
                    }
                    else
                    {
                        TheIdealShipPlugin.isHttps.Value = true;
                    }
                    Color isHttpsColor = TheIdealShipPlugin.isHttps.Value ? Palette.AcceptedGreen : Palette.White;
                    isHttpsPassiveButton.OnMouseOut.AddListener((Action)(() => isHttpsButtonSprite.color = isHttpsColor));
                    UpdateRegions();
                }
            }

        }

        // This is part of the Mini.RegionInstaller, Licensed under GPLv3
        // file="RegionInstallPlugin.cs" company="miniduikboot">

        public static void UpdateRegions()
        {
            string serverIp = (TheIdealShipPlugin.isHttps.Value ? "https://" : "http://" ) + TheIdealShipPlugin.CustomIp.Value;
            ServerInfo MCCNServer = new ServerInfo("MC-CN","http://au.pafyx.top",22000,false);
            ServerInfo serverInfo = new ServerInfo("Custom", serverIp, TheIdealShipPlugin.CustomPort.Value, false);
            ServerInfo[] SInfo = new ServerInfo[] {serverInfo};
            ServerInfo[] MSInfo = new ServerInfo[] {MCCNServer};
            ServerManager serverManager = FastDestroyableSingleton<ServerManager>.Instance;
            var regions = new IRegionInfo[] {
                new StaticHttpRegionInfo("Custom", StringNames.NoTranslation, serverIp, SInfo).CastFast<IRegionInfo>(),
                new StaticHttpRegionInfo("MC-CN", StringNames.NoTranslation, "http://au.pafyx.top", MSInfo).CastFast<IRegionInfo>()
            };

            IRegionInfo currentRegion = serverManager.CurrentRegion;
            foreach (IRegionInfo region in regions)
            {
                if (currentRegion != null && region.Name.Equals(currentRegion.Name, StringComparison.OrdinalIgnoreCase))
                    currentRegion = region;
                    serverManager.AddOrUpdateRegion(region);
            }

            // AU remembers the previous region that was set, so we need to restore it
            if (currentRegion != null)
            {
                serverManager.SetRegion(currentRegion);
            }
        }
    }
}
