using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using System;
using Reactor.Networking.Attributes;
using TheIdealShip.Languages;
using TheIdealShip.Roles;
using TheIdealShip.Patches;

namespace TheIdealShip
{
    [BepInPlugin(Id, ModName, VersionString)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    [ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
    public class TheIdealShipPlugin : BasePlugin
    {
        // Among Us游玩版本
        public const string AmongUsVersion = "12.14";
        // 模组名称
        public const string ModName = "The Ideal Ship";
        // 模组id
        public const string Id = "me.huier.TheIdealShip";
        // 模组版本
        public const string VersionString = "0.3.2";
    /*
        // 模组构建时间
        public const string BuildTime = "";
        // 是否为开发版本
        public const bool IsDev = true;
    */
        // Github链接
        public const string GithubURL = "https://github.com/TheIdealShipAU/TheIdealShip";
        // Discord服务器链接
        public const string DiscordURL = "https://discord.gg/PrjetphRxh";
        // bilibili链接
        public const string bilibiliURL = "https://space.bilibili.com/394107547";
        // KOOK链接
        public const string KOOKURL = "https://kook.top/T9DTrC";
        public static Version Version = Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
        public Harmony Harmony { get; } = new Harmony(Id);
        public static TheIdealShipPlugin Instance;

        public ConfigEntry<string> ConfigName { get; private set; }
        public static ConfigEntry<string> CustomIp { get; set; }
        public static ConfigEntry<ushort> CustomPort { get; set; }
        public static ConfigEntry<bool> isHttps { get; set;}

        public override void Load()
        {
            Logger = Log;
            Instance = this;

            CustomIp = Config.Bind("Custom", "Custom Server IP", "127.0.0.1");
            CustomPort = Config.Bind("Custom", "Custom Server Port", (ushort)22000);
            isHttps = Config.Bind("Custom","Custom Server isHttps", false);
            RegionMenuOpenPatch.UpdateRegions();

            Harmony.PatchAll();

            CustomOptionHolder.Load();
            RoleInfo.Init();


            Language.Init();
            LanguagePack.Init();
        }
    }
}