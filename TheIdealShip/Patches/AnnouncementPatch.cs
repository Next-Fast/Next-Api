using System;
using HarmonyLib;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using TheIdealShip.Modules;

namespace TheIdealShip.Patches
{

    [HarmonyPatch(typeof(AnnouncementPopUp),nameof(AnnouncementPopUp.UpdateAnnounceText))]
    public static class AnnouncementPatch
    {
        public static string AMText = "我是一个大笨蛋";
        public static bool Prefix(AnnouncementPopUp __instance)
        {
//            if (!ModUpdater.HUpdate) return true;
            __instance.AnnounceTextMeshPro.text = AMText;
            return false;
        }

    }
}
