using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using NextShip.Manager;
using NextShip.Utilities.Attributes;

namespace NextShip.Plugins;

public static class PluginManager
{
    public const string PluginsPath = "./Plugins";
    public static List<string> PluginPathS = new List<string>();
    public static List<(Assembly, Type, ShipPluginInfo)> PluginS = new List<(Assembly, Type, ShipPluginInfo)>();
    public static bool existDirectory;
    
    [Init]
    public static void Init()
    {
        existDirectory = FilesManager.CreateDirectory(PluginsPath);
        LoadPlugins();
    }

    public static void LoadPlugins()
    {
        PluginPathS = FindPlugins();
        if (PluginPathS == null) return;
        PluginPathS.Do(Load);
        PluginS.Do(n => n.Item2.Load(n.Item3));
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
        var types = assembly.GetTypes();
        var has = false;
        var IsInherit = false; 
        types.Do
        (n =>
            {
                ShipPlugin shipPlugin = n.GetCustomAttribute<ShipPlugin>();
                IsInherit = n.BaseType == typeof(ShipPlugin);
                if (shipPlugin != null && IsInherit)
                {
                    PluginS.Add((assembly, n, shipPlugin.ShipPluginInfo));
                    has = true;
                }
            }
        );
        
        if (!has)
        {
            Info($"{fileName} 未注册插件", filename: MethodUtils.GetClassName());
        }

        if (!IsInherit)
        {
            Info($"{fileName} no Inherit", filename: MethodUtils.GetClassName());
        }
    }

    public static void Load(this Type type, ShipPluginInfo shipPluginInfo)
    {
        var method = type.GetMethod("Load");
        if (method == null)
        {
            return;
        }

        try
        {
            method.Invoke(null,null);
            Info($"Name:{shipPluginInfo.Name} . Version:{shipPluginInfo.Version} . Id:{shipPluginInfo.Id} 运行成功 ", filename: MethodUtils.GetClassName());
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }

    public static List<string> FindPlugins()
    {
        List<string> pluginPaths = new List<string>();
        DirectoryInfo plugins = new DirectoryInfo(PluginsPath);
        FileInfo[] fileInfos = plugins.GetFiles();
        fileInfos.Do(n => pluginPaths.Add(n.FullName));
        return pluginPaths;
    }

    public static List<ShipPlugin> Plugins = new List<ShipPlugin>();
}