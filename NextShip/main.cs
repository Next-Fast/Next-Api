using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using NextShip.Cosmetics;
using NextShip.Languages;
using NextShip.Manager;
using NextShip.Patches;
using NextShip.Roles;
using NextShip.UI.Components;
using NextShip.UI.UIManager;
using UnityEngine;
using Action = Il2CppSystem.Action;
using Object = UnityEngine.Object;

[assembly: AssemblyFileVersion(Main.VersionString)]
[assembly: AssemblyInformationalVersion(Main.VersionString)]

namespace NextShip;

[BepInPlugin(Id, ModName, VersionString)]
[BepInProcess("Among Us.exe")]
public sealed class Main : BasePlugin
{
    // Among Us游玩版本
    public static readonly AmongUsVersion SupportVersion = new AmongUsVersion(2023, 7, 21);
    public static readonly AmongUsVersion[] AmongUsSupportVersions = new[]
    {
        new AmongUsVersion(2023, 7, 21) 
    };

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
    public static readonly string HashCode = HashUtils.GetFileMD5Hash(Paths.PluginPath.CombinePath($"{ModName}.dll"));

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
    internal static NextUIManager _nextUIManager;
    internal static readonly ServerManager serverManager = DestroyableSingleton<ServerManager>.Instance;

    // 模组主颜色
    public readonly Color ModColor = "#90c2f4".HTMLColorTo32();
    internal Harmony Harmony { get; } = new(Id);

    private static void AddQuit()
    {
        Process.GetCurrentProcess().Exited += (sender, args) => OnGameQuit();
        Application.add_quitting((Action)(() => OnGameQuit()));
    }
    public override void Load()
    {
        AddQuit();
        
        if (IsDev) 
            ConsoleTextFC();
        
        ConsoleManager.SetConsoleTitle("Among Us " + ModName + " Game");
        TISLog = BepInEx.Logging.Logger.CreateLogSource(ModName.RemoveBlank());

        Instance = this;
        Harmony.PatchAll();

        FilesManager.Init();
        ServerPath.autoAddServer();

        var _Assembly = Assembly.GetExecutingAssembly();
        RegisterManager.Registration(_Assembly);
        
        UpdateTask = AddComponent<UpdateTask>();
        _nextUIManager ??= AddComponent<NextUIManager>();

        Init();
        LanguagePack.Init();
        
        Object.DontDestroyOnLoad(UpdateTask);
        Object.DontDestroyOnLoad(_nextUIManager);
        
        CustomCosmeticsManager.LoadHat();
        
        RegisterRoles();
        
        VanillaManager.Load();
    }
    

    private static void RegisterRoles()
    {
        var roles = new Role[]
        {
            new Postman()
        };

        Roles.RoleManager.Get().RegisterRole(roles);
    }

    private static void constInit()
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
        Info($"Support Among Us Version {SupportVersion}", "Info");
        Info($"Hash: {HashCode}", "Info");
        Info($"欢迎游玩{ModName} | Welcome to{ModName}", "Info");
    }

    private static void OnGameQuit()
    {
        OutputTISLog();
    }
}