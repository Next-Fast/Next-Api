using System.Collections.Generic;
using NextShip.Manager;

namespace NextShip.Plugins;

public class PluginManager
{
    public const string PluginsPath = "./Plugins";
    public static bool existDirectory;

    public static void Init()
    {
        existDirectory = FilesManager.CreateDirectory(PluginsPath);
    }

    public static List<ShipPlugin> Plugins = new List<ShipPlugin>();
}