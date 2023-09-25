using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using NextShip.Manager;
using NextShip.Utilities.Attributes;

namespace NextShip.Plugins;

public static class PluginManager
{
    private static string PluginsPath;
    private static List<string> PluginPathS = new();
    private static List<(Assembly, Type, ShipPlugin)> PluginCreateS = new();
    public static bool existDirectory;

    public static List<ShipPlugin> Plugins = new();

    [Load]
    public static void Init()
    {
        PluginsPath = FilesManager.GetCreativityDirectory("Plugins").FullName;
        existDirectory = Directory.Exists(PluginsPath);
        
        LoadPlugins();
    }

    private static void LoadPlugins()
    {
        PluginPathS = FindPlugins();
        if (PluginPathS == null) return;
        PluginPathS.Do(Load);
        
        PluginCreateS.Do(n => n.Load());
    }

    public static void Load(this string path)
    {
        var fileName = Path.GetFileName(path);
        if (!path.Contains(".dll"))
        {
            Info($"{fileName} 疑似不是dll", filename: MethodUtils.GetClassName());
            return;
        }

        var assembly = Assembly.LoadFile(path);
        var types = assembly.GetTypes().Where(n => n.BaseType == typeof(ShipPlugin));
        var has = false;
        var IsInherit = false;
        types.Do
        (n =>
            {
                IsInherit = true;
                has = true;
                
                var plugin = (ShipPlugin)assembly.CreateInstance(n.FullName!);
                if (plugin?.ShipPluginInfo == null)
                {
                    var shipPluginInfo = n.GetCustomAttribute<ShipPluginInfo>();
                    plugin!.ShipPluginInfo = shipPluginInfo;
                }
                
                var Compatibilities = n.GetCustomAttributes<PluginCompatibility>();
                plugin.PluginCompatibilities = Compatibilities.ToList();
                
                PluginCreateS.Add((assembly, n, plugin));
            }
        );
        if (!has) Info($"{fileName} 未注册插件", filename: MethodUtils.GetClassName());

        if (!IsInherit) Info($"{fileName} no Inherit", filename: MethodUtils.GetClassName());
    }

    private static void Load(this (Assembly, Type, ShipPlugin) pluginTuple)
    {
        var shipPluginInfo = pluginTuple.Item3.ShipPluginInfo;
        try
        {
            pluginTuple.Item3.Load();            
            Info($"Name:{shipPluginInfo.Name} . Version:{shipPluginInfo.Version} . Id:{shipPluginInfo.Id} 运行成功 ",
                filename: MethodUtils.GetClassName());
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }

    private static List<string> FindPlugins()
    {
        var pluginPaths = new List<string>();
        var plugins = new DirectoryInfo(PluginsPath);
        var fileInfos = plugins.GetFiles();
        fileInfos.Do(n => pluginPaths.Add(n.FullName));
        return pluginPaths;
    }
}