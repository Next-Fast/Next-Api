global using static NextShip.Modules.log;
global using Main = NextShip.Main;
global using NextShip.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime;
using NextShip.Languages;
using NextShip.Manager;
using NextShip.Patches;
using NextShip.UI.Components;
using NextShip.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

[assembly: AssemblyFileVersion(Main.VersionString)]
[assembly: AssemblyInformationalVersion(Main.VersionString)]

namespace NextShip;

[BepInPlugin(Id, ModName, VersionString)]
[BepInProcess("Among Us.exe")]
public class Main : BasePlugin
{
    // Among Us游玩版本
    public const string AmongUsVersion = "2023.7.11";

    // 模组名称
    public const string ModName = "NextShip";

    // 模组id
    public const string Id = "cn.MengChu.NextShip";

    // 模组版本
    public const string VersionString = "0.5.0";

    // bilibili链接
    public const string bilibiliURL = "https://space.bilibili.com/394107547";

    public const string QQURL =
        "http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=OqTfMLjm7lMD5OMV68Rs9JLnbcXc-fDR&authKey=8tt0sNVVsfsGvOBFLNmtDA8CD7fweh%2Bbe1%2FMq2j62zGNWJ17Q%2FNXfG4c7r6JlN1S&noverify=0&group_code=815101721";

    public const string QQNumber = "815101721";

    // 模组构建时间
    public static string BuildTime = "";

    // 是否为开发版本
    public static bool IsDev = true;

    internal static bool isCn;
    internal static bool isChinese;

    public static Version Version = Version.Parse(VersionString);
    internal static ManualLogSource TISLog;
    public static Main Instance;

    internal static UpdateTask UpdateTask;
    internal static ControlManager _ControlManager;
    internal static readonly ServerManager serverManager = DestroyableSingleton<ServerManager>.Instance;

    // 模组主颜色
    public readonly Color ModColor = "#90c2f4".HTMLColorTo32();
    internal Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        TISLog = BepInEx.Logging.Logger.CreateLogSource(ModName.RemoveBlank());
        ConsoleManager.SetConsoleTitle(("Among Us " + ModName + " Game").ToColorString(ModColor));
        Instance = this;
        Harmony.PatchAll();

        if (IsDev) ConsoleTextFC();
        constInit();

        FilesManager.Init();
        ServerPath.autoAddServer();

        var _Assembly = Assembly.GetExecutingAssembly();
        RegisterManager.Registration(_Assembly);
        AssetLoader _assetLoader = new AssetLoader(_Assembly);
        UpdateTask = AddComponent<UpdateTask>();

        Init();
        LanguagePack.Init();
        ObjetUtils.Do(new Object[]{ UpdateTask });
        /*TaskUtils.StartTask(new[] { OptionManager.Load});*/
    }


    public static void constInit()
    {
#if RELEASE
            IsDev = false;
#endif

        var CountryName = RegionInfo.CurrentRegion.EnglishName;
        isCn = CountryName.Contains("China"); //|| CountryName.Contains("Hong Kong") || CountryName.Contains("Taiwan");
        isChinese = (TranslationController.InstanceExists
            ? TranslationController.Instance.currentLanguage.languageID
            : SupportedLangs.English) is SupportedLangs.SChinese or SupportedLangs.TChinese;

        Info($"IsDev:{IsDev.ToString()}", "Const");
        Info($"CountryName:{CountryName} | {RegionInfo.CurrentRegion.DisplayName}", "Const");
        Info($"isCn:{isCn.ToString()}", "Const");
        Info($"IsChinese:{isChinese.ToString()}", "Const");
        Info($"Support Among Us Version {AmongUsVersion}", "Info");
        Info($"Hash: {HashUtils.GetFileMD5Hash(Path.Combine(Paths.PluginPath , $"{ModName}.dll"))}", "Info");
        Info($"欢迎游玩{ModName} | Welcome to{ModName}", "Info");
    }
}