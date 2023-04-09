using System;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using TheIdealShip.Utilities;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public class RegionMenuPatch
    {
        private static ServerManager serverManager = FastDestroyableSingleton<ServerManager>.Instance;
        [HarmonyPatch(typeof(RegionMenu), nameof(RegionMenu.OnEnable)), HarmonyPrefix]
        public static bool RegionMenuOnEnablePatch(RegionMenu __instance)
        {
            if (serverManager.AvailableRegions.Count <= 6) return true;
            CreateServerOption(__instance);
            return false;
        }

        private static void CreateServerOption(RegionMenu __instance)
        {
            __instance.controllerSelectable.Clear();
            int num = 0;
            foreach (IRegionInfo regionInfo in serverManager.AvailableRegions)
            {
                IRegionInfo region = regionInfo;
                ServerListButton serverListButton = __instance.ButtonPool.Get<ServerListButton>();
                serverListButton.transform.localPosition = new Vector3(0f, 2f - 0.5f * (float)num, 0f);
                serverListButton.SetTextTranslationId(regionInfo.TranslateName, regionInfo.Name);
                serverListButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(regionInfo.TranslateName, regionInfo.Name);
                serverListButton.Text.ForceMeshUpdate(false, false);
                serverListButton.Button.OnClick.RemoveAllListeners();
                serverListButton.Button.OnClick.AddListener((UnityAction)(() =>
                {
                    __instance.ChooseOption(region);
                }));
                serverListButton.SetSelected(DestroyableSingleton<ServerManager>.Instance.CurrentRegion.Equals(regionInfo));
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
            ControllerManager.Instance.OpenOverlayMenu(__instance.name, __instance.BackButton, __instance.defaultButtonSelected, __instance.controllerSelectable, false);
        }

/*         public static void CreateSwitchServerPages(RegionMenu __instance)
        {
            GameObject Previous = new GameObject().AddComponent<back>
            GameObject Next = new GameObject
        } */
        
    }
}