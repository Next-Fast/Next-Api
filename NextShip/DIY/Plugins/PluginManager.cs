using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using NextShip.Api.Attributes;
using NextShip.Api.Bases;
using NextShip.Manager;

namespace NextShip.DIY.Plugins;

public class PluginManager : Manager<PluginManager>
{
    private readonly List<(Assembly, Type, ShipPlugin)> PluginCreateS = new();
    public bool existDirectory;
    private List<string> PluginPathS = new();

    public List<ShipPlugin> Plugins = new();
    private string PluginsPath;

    [Load]
    public static void Init()
    {
        Get().Load();
    }

    private void Load()
    {
        PluginsPath = FilesManager.GetCreativityDirectory("Plugins").FullName;
        existDirectory = Directory.Exists(PluginsPath);

        LoadPlugins();
    }

    private void LoadPlugins()
    {
        PluginPathS = FindPlugins();
        if (PluginPathS == null) return;

        PluginPathS.Do(Load);
        PluginCreateS.Do(Load);
    }

    private void Load(string path)
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
                if (plugin == null)
                    return;

                var shipPluginInfo = n.GetCustomAttribute<ShipPluginInfo>();
                if (shipPluginInfo != null)
                    plugin.ShipPluginInfo = shipPluginInfo;

                var Compatibilities = n.GetCustomAttributes<PluginCompatibility>();
                plugin.PluginCompatibilities = Compatibilities.ToList();

                PluginCreateS.Add((assembly, n, plugin));
            }
        );
        if (!has) Info($"{fileName} 未注册插件", filename: MethodUtils.GetClassName());

        if (!IsInherit) Info($"{fileName} no Inherit", filename: MethodUtils.GetClassName());
    }

    private static void Load((Assembly, Type, ShipPlugin) pluginTuple)
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

    private List<string> FindPlugins()
    {
        var pluginPaths = new List<string>();
        var plugins = new DirectoryInfo(PluginsPath);
        var fileInfos = plugins.GetFiles();
        fileInfos.Do(n => pluginPaths.Add(n.FullName));
        return pluginPaths;
    }
}