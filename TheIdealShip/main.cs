global using static TheIdealShip.Modules.log;
global using Main = TheIdealShip.TheIdealShipPlugin;

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using TheIdealShip.Languages;
using TheIdealShip.Patches;
using TheIdealShip.Manager;

[assembly: AssemblyFileVersionAttribute(TheIdealShip.TheIdealShipPlugin.VersionString)]
[assembly: AssemblyInformationalVersionAttribute(TheIdealShip.TheIdealShipPlugin.VersionString)]
namespace TheIdealShip
{
    [BepInPlugin(Id, ModName, VersionString)]
    [BepInProcess("Among Us.exe")]
    public class TheIdealShipPlugin : BasePlugin
    {
        // Among Us游玩版本
        public const string AmongUsVersion = "2023.3.28";
        // 模组名称
        public const string ModName = "The Ideal Ship";
        // 模组id
        public const string Id = "me.huier.TheIdealShip";
        // 模组版本
        public const string VersionString = "0.3.6";
        // 模组构建时间
        public static string BuildTime = "2023.3.8";
        // 是否为开发版本
        public static bool IsDev = true;
        public static bool isCn;
        public static bool isChinese;
        // Github链接
        public const string GithubURL = "https://github.com/TheIdealShipAU/TheIdealShip";
        // bilibili链接
        public const string bilibiliURL = "https://space.bilibili.com/394107547";
        public const string QQURL = "http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=OqTfMLjm7lMD5OMV68Rs9JLnbcXc-fDR&authKey=8tt0sNVVsfsGvOBFLNmtDA8CD7fweh%2Bbe1%2FMq2j62zGNWJ17Q%2FNXfG4c7r6JlN1S&noverify=0&group_code=815101721";
        public const string QQNumber = "815101721";
        // Gitee链接
        public const string GiteeURL = "https://gitee.com/mc-huier-sgss/TheIdealShip";
        public static Version Version = Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
        public Harmony Harmony { get; } = new Harmony(Id);
        public static TheIdealShipPlugin Instance;
        public static int OptionPage = 0;
        public static Dictionary<byte, RoleId> PlayerAndRoleIdDic = new Dictionary<byte, RoleId>();

        public override void Load()
        {
            Logger = Log;
            Instance = this;
            Harmony.PatchAll();

#if DEBUG
            Modules.log.ConsoleTextFC();
#endif

            constInit();

            CustomOptionHolder.Load();
/*             RoleInfo.Init(); */
            FilesManager.Init();
            RegionMenuOpenPatch.autoAddServer();

            Language.Init();
            LanguagePack.Init();
        }


        public static void constInit()
        {
            #if RELEASE
            IsDev = false;
            #endif


            /* uint langId = AmongUs.Data.Legacy.LegacySaveManager.LastLanguage;
            isChinese = (langId == 13 || langId == 14); */

            var CountryName = RegionInfo.CurrentRegion.EnglishName;
            isCn = CountryName.Contains("China");//|| CountryName.Contains("Hong Kong") || CountryName.Contains("Taiwan");

            Info($"IsDev:{IsDev.ToString()}", "Const");
            /* Info($"LanguageId:{langId.ToString()}", "Const"); */
            Info($"CountryName:{CountryName} | {RegionInfo.CurrentRegion.DisplayName}", "Const");
            Info($"isCn:{isCn.ToString()}", "Const");
            Info($"IsChinese:{isChinese.ToString()}", "Const");
        }
    }
}