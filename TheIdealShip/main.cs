global using static TheIdealShip.Modules.log;
global using Main = TheIdealShip.TheIdealShipPlugin;

using System.Collections.Generic;
using System.Globalization;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using System;
using Reactor.Networking.Attributes;
using TheIdealShip.Languages;
using TheIdealShip.Roles;

namespace TheIdealShip
{
    [BepInPlugin(Id, ModName, VersionString)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    [ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
    public class TheIdealShipPlugin : BasePlugin
    {
        // Among Us游玩版本
        public const string AmongUsVersion = "2023.2.28";
        // 模组名称
        public const string ModName = "The Ideal Ship";
        // 模组id
        public const string Id = "me.huier.TheIdealShip";
        // 模组版本
        public const string VersionString = "0.3.5";
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
        // KOOK链接
        public const string QQURL = "https://pd.qq.com/s/hpldx5uja";
        public static Version Version = Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
        public Harmony Harmony { get; } = new Harmony(Id);
        public static TheIdealShipPlugin Instance;
        public static int OptionPage = 0;
        public static Dictionary<byte, RoleId> PlayerAndRoleIdDic;

        public override void Load()
        {
            Logger = Log;
            Instance = this;
            Harmony.PatchAll();

            Modules.log.ConsoleTextFC();
            constInit();

            CustomOptionHolder.Load();
            RoleInfo.Init();


            Language.Init();
            LanguagePack.Init();
        }


        public static void constInit()
        {
            #if RELEASE
            IsDev = false;
            #endif

            uint langId = AmongUs.Data.Legacy.LegacySaveManager.LastLanguage;
            isChinese = (langId == 13 || langId == 14);

            var CountryName = RegionInfo.CurrentRegion.EnglishName;
            isCn = CountryName.Contains("China");//|| CountryName.Contains("Hong Kong") || CountryName.Contains("Taiwan");

            Info($"IsDev:{IsDev.ToString()}", "Const");
            Info($"LanguageId:{langId.ToString()}", "Const");
            Info($"CountryName:{CountryName} | {RegionInfo.CurrentRegion.DisplayName}", "Const");
            Info($"isCn:{isCn.ToString()}", "Const");
            Info($"IsChinese:{isChinese.ToString()}", "Const");
        }
    }
}