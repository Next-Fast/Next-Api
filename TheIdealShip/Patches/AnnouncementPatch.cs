
using System.Net.Mime;
using HarmonyLib;
using Announcement = Assets.InnerNet.Announcement;
using AmongUs.Data.Player;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using TheIdealShip.Modules;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public class AnnouncementPatch
    {

        //https://github.com/Dolly1016/Nebula/blob/master/Nebula/Module/ModUpdater.cs
        public static List<Announcement> modUpdateAn;
        [HarmonyPatch(typeof(PlayerAnnouncementData), nameof(PlayerAnnouncementData.SetAnnouncements)), HarmonyPrefix]
        public static void SetModAnnouncements(PlayerAnnouncementData __instance, [HarmonyArgument(0)] ref Il2CppReferenceArray<Announcement> aRange)
        {
            if (modUpdateAn == null) return;
            List<Announcement> list = new(aRange.ToList());
            list.AddRange(modUpdateAn);

            list.Sort((a1 , a2) => AnCompare(a1, a2));
            aRange = list.ToArray();
        }

        public static void AddAnnouncement(Announcement an)
        {
            if (modUpdateAn.Count >= 5) modUpdateAn.RemoveAt(0);
            modUpdateAn.Add(an);
        }

        public static int AnCompare(Announcement an1, Announcement an2)
        {
            string[] time1 = an1.Date.Split('-');
            string[] time2 = an2.Date.Split('-');
            int Sort;
            for (int i = 0; i < 3; i++)
            {
                var t1 = int.Parse(time1[i]);
                var t2 = int.Parse(time2[i]);

                if (t1 > t2)
                {
                    Sort = 1;
                    return Sort;
                }
                if (t1 < t2)
                {
                    Sort = -1;
                    return Sort;
                }
                if (t1 == t2) continue;
            }
            return 0;
        }
    }

    public class ModAnnouncement
    {
        public string text;
        public string Time;
        public string Title;
        public string SubTitle;
        public string ShortTitle;
        public uint langid;
        ModAnnouncement
        (
            string text,
            string Title,
            string Time,
            string SubTitle = "",
            string ShortTitle = "",
            uint langid = 13
        )
        {
            this.text = text;
            this.Title = Title;
            this.langid = langid;
            this.SubTitle = SubTitle;
            this.ShortTitle = ShortTitle;
            this.Time = Time.Replace('.', '-');
        }
        public Announcement ToAn(ModAnnouncement modAn)
        {
            Announcement an = new Announcement();
            an.Id = "mod";
            an.Language = modAn.langid;
            an.Number = AnnouncementPatch.modUpdateAn != null ? AnnouncementPatch.modUpdateAn[AnnouncementPatch.modUpdateAn.Count].Number + 1 : 1000;
            an.Text = modAn.text;
            an.SubTitle = modAn.SubTitle;
            an.ShortTitle = modAn.ShortTitle;
            an.Title = modAn.Title;
            an.Date = modAn.Time;
            return an;
        }
    }
}
