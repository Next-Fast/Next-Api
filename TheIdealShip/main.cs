using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using System;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace TheIdealShip;

[BepInPlugin(Id, "The Ideal Ship", VersionString)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
public partial class TheIdealShipPlugin : BasePlugin
{
    public const string Id = "me.huier.TheIdealShip";
    public const string VersionString ="1.0.0";
    public static Version Version = Version.Parse(VersionString);
    public Harmony Harmony { get; } = new Harmony(Id);

    public ConfigEntry<string> ConfigName { get; private set; }

    public override void Load()
    {

    }
}