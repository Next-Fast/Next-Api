using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Microsoft.Extensions.DependencyInjection;
using NextShip.Api.Attributes;
using NextShip.Api.Extension;
using NextShip.Api.Interfaces;
using NextShip.Api.Services;
using NextShip.Cosmetics;
using NextShip.DIY.Plugins;
using NextShip.Languages;
using NextShip.Manager;
using NextShip.Patches;
using NextShip.Services;
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
    public const string VersionString = "1.0.0";

    // Among Us游玩版本
    public static readonly AmongUsVersion SupportVersion = new(2023, 10, 24);

    internal static readonly ServerManager serverManager = FastDestroyableSingleton<ServerManager>.Instance;

    private ManualLogSource TISLog;

    public static NextService _Service { get; private set; }

    public static Main Instance { get; private set; }

    public static Assembly RootAssembly { get; private set; }

    private Harmony Harmony { get; } = new(Id);


    public override void Load()
    {
        Instance = this;
        RootAssembly = Assembly.GetExecutingAssembly();
        TISLog = BepInExLogger.CreateLogSource(ModName.RemoveBlank());
        Harmony.PatchAll();

        PluginManager.Get().Load();
        
        Init();
        Get(TISLog);
        CreateService();

        SteamExtension.UseSteamIdFile();
        ReactorExtension.UseReactorHandshake();

        ConsoleManager.SetConsoleTitle("Among Us " + ModName + " Game");
        RegisterManager.Registration();

        AddComponent<NextManager>().DontDestroyOnLoad();
        
        ServerPath.autoAddServer();
        LanguagePack.Init();
        CustomCosmeticsManager.LoadHat();
    }

    private static void Init()
    {
        Info("IsDev:{}", "Const");
        Info($"CountryName:| {RegionInfo.CurrentRegion.DisplayName}", "Const");
        Info("isCn:", "Const");
        Info("IsChinese:", "Const");
        Info($"Support Among Us Version {SupportVersion}", "Info");
        Info("Hash: ", "Info");
        Info($"欢迎游玩{ModName} | Welcome to{ModName}", "Info");
    }


    private static void CreateService()
    {
        var builder = new ServiceBuilder();
        builder.CreateService();
        builder._collection.AddLogging();
        builder._collection.AddSingleton<IRoleManager, NextRoleManager>();
        builder._collection.AddSingleton<IEventManager, EventManager>();
        builder._collection.AddSingleton<IPatchManager, NextPatchManager>();
        builder._collection.AddSingleton<IPlayerManager, NextPlayerManager>();
        builder.AddTransient<HttpClient>();
        builder.Add<DownloadService>();
        builder.Add<MetadataService>();
        builder.Add<HatService>();
        builder.Add<DataService>();
        ServiceAdd(builder, RootAssembly);

        _Service = NextService.Build(builder);
        ServiceAddAttribute.Registration(_Service._Provider, RootAssembly);
    }

    private static void ServiceAdd(IServiceBuilder builder, Assembly addAssembly)
    {
        var types = addAssembly.GetTypes().Where(n => n.IsDefined(typeof(INextServiceAdd)))
            .Select(AccessTools.CreateInstance).Select(n => (INextServiceAdd)n);
        foreach (var varType in types) varType.ServiceAdd(builder);
    }
}