global using NextShip.Updates;
using System.Threading.Tasks;
using BepInEx.Configuration;

namespace NextShip.Updates;

public class ModUpdater
{
    // HUdate=HasUpdate 判断是否有更新
    public static bool HUpdate = VersionManager.HUpdate;
    public static ConfigEntry<bool> AutoUpdate;

    // 模组下载链接
    public string ModDownloadURL;


    public ModUpdater(string modDownloadURL)
    {
        ModDownloadURL = modDownloadURL;
    }

    public Task<bool> UpdateMod()
    {
        return Task.FromResult(true);
    }
}