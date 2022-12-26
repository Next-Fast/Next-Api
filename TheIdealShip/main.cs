using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using System;
using Reactor.Networking.Attributes;
using TheIdealShip.Languages;

namespace TheIdealShip;

[BepInPlugin(Id, ModName, VersionString)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
public partial class TheIdealShipPlugin : BasePlugin
{
    // Among Us游玩版本
    public const string AmongUsVersion = "12.8";
    // 模组名称
    public const string ModName = "The Ideal Ship";
    // 模组id
    public const string Id = "me.huier.TheIdealShip";
    // 模组版本
    public const string VersionString = "1.0.0";
    // 模组构建时间
    public const string BuildTime = "";
    // 是否为开发版本
    public const bool IsDev = true;
    // Github链接
    public const string GithubURL = "https://github.com/TheIdealShipAU/TheIdealShip";
    // Discord服务器链接
    public const string DiscordURL = "https://discord.gg/PrjetphRxh";
    // bilibili链接
    public const string bilibiliURL = "https://space.bilibili.com/394107547";
    // KOOK链接
    public const string KOOKURL = "";
    public static Version Version = Version.Parse(VersionString);
    public Harmony Harmony { get; } = new Harmony(Id);
    public static TheIdealShipPlugin Instance;

    public ConfigEntry<string> ConfigName { get; private set; }

    public override void Load()
    {
        Instance = this;


        LanguageCSV.Init();
        LanguagePack.Init();

        Harmony.PatchAll();
    }
}