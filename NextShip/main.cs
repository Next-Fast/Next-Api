using System.Globalization;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using NextShip.Cosmetics;
using NextShip.Languages;
using NextShip.Manager;
using NextShip.Patches;
using NextShip.UI.UIManager;

using BepInExLogger = BepInEx.Logging.Logger;

[assembly: AssemblyFileVersion(Main.VersionString)]
[assembly: AssemblyInformationalVersion(Main.VersionString)]

namespace NextShip;

[BepInPlugin(Id, ModName, VersionString)]
[BepInProcess("Among Us.exe")]
public sealed class NextShip : BasePlugin
{
    // 模组名称
    public const string ModName = "NextShip";

    // 模组id
    public const string Id = "cn.MengChu.NextShip";

    // 模组版本
    public const string VersionString = "100.0";

    // Among Us游玩版本
    public static readonly AmongUsVersion SupportVersion = new(2023, 10, 24);
    
    
    internal static ManualLogSource TISLog;
    
    public static NextShip Instance;
    
    internal static readonly ServerManager serverManager = FastDestroyableSingleton<ServerManager>.Instance;
    private Harmony Harmony { get; } = new(Id);


    public override void Load()
    {
        ConsoleManager.SetConsoleTitle("Among Us " + ModName + " Game");
        TISLog = BepInExLogger.CreateLogSource(ModName.RemoveBlank());

        Init();
        Get(TISLog);

        Instance = this;
        Harmony.PatchAll();

        FilesManager.Init();
        ServerPath.autoAddServer();
        
        RegisterManager.Registration();
        
        AddComponent<NextUIManager>();
        
        LanguagePack.Init();
        
        CustomCosmeticsManager.LoadHat();
    }

    private static void Init()
    {
        Info("IsDev:{}", "Const");
        Info($"CountryName:| {RegionInfo.CurrentRegion.DisplayName}", "Const");
        Info("isCn:", "Const");
        Info($"IsChinese:", "Const");
        Info($"Support Among Us Version {SupportVersion}", "Info");
        Info($"Hash: ", "Info");
        Info($"欢迎游玩{ModName} | Welcome to{ModName}", "Info");
    }
}