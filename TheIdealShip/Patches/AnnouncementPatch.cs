/* using HarmonyLib;
using TheIdealShip.Modules;
using static TheIdealShip.Helpers;
using AmongUs.Data;
using Announcement = Assets.InnerNet.Announcement;
using Il2CppSystem.Collections.Generic;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public class AnnouncementPatch
    {
        [HarmonyPatch(typeof(AnnouncementPopUp), nameof(AnnouncementPopUp.CreateAnnouncementList))]
        public class AnnouncementCreateListPatch
        {
            private static List<Announcement> AnList = new List<Announcement>();
            public static void Pfix()
            {
                var allan = DataManager.Player.Announcements.AllAnnouncements;
/*                 if (!ModUpdater.HUpdate) return;
                Announcement an = new Announcement();
                an.Id = allan[0].Id + 1;
                an.Language = allan[0].Language;
                an.Number = 1;
                an.Title = "TheIdealShip更新公告";
                an.SubTitle = "测试而已";
                an.ShortTitle = an.Title;
                an.PinState = allan[0].PinState;
                an.Text = "测试而已";
                an.Date = "2023.3.1";
                AnList.Add(allan[0]);
                DataManager.Player.Announcements.AllAnnouncements.Clear();
                foreach (var r in AnList)
                {
                    DataManager.Player.Announcements.AllAnnouncements.Add(r);
                }
            }
        }
    }
} */
