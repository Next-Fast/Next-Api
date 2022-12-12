using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Utilities;

namespace TheIdealShip;

[BepInAutoPlugin(Id,ModName,ModVersion)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class TheIdealShipPlugin : BasePlugin
{
    public const string Id = "me.huier.TheIdealShip";
    public const string ModName ="The Ideal Ship";
    public const string ModVersion ="0.0.1";
    public Harmony Harmony { get; } = new(Id);

    public ConfigEntry<string> ConfigName { get; private set; }

    public override void Load()
    {

    }
}