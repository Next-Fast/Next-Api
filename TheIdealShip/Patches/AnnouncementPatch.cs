using HarmonyLib;
using Announcement = Assets.InnerNet.Announcement;
using Il2CppSystem.Collections.Generic;
using AmongUs.Data.Player;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using TheIdealShip.Modules;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public class AnnouncementPatch
    {
        public static List<Announcement> modUpdateAn;
        [HarmonyPatch(typeof(PlayerAnnouncementData), nameof(PlayerAnnouncementData.SetAnnouncements)), HarmonyPrefix]
        public static bool SetModAnnouncements(PlayerAnnouncementData __instance, [HarmonyArgument(0)] Il2CppReferenceArray<Announcement> aRange)
        {
            if (!ModUpdater.HUpdate) return true;

            List<Announcement> list = new();
            foreach (var a in aRange) list.Add(a);
            if (modUpdateAn != null) foreach (var a in modUpdateAn) list.Add(a);

            __instance.allAnnouncements = new List<Announcement>();
            foreach (var a in list) __instance.allAnnouncements.Add(a);

            __instance.HandleChange();
            __instance.OnAddAnnouncement?.Invoke();

            return false;
        }
    }
}
