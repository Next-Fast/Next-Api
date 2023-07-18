global using NextShip.Updates;
using BepInEx.Configuration;

namespace NextShip.Updates;

public static class ModUpdater
{
    // HUdate=HasUpdate 判断是否有更新
    public static bool HUpdate = VersionManager.HUpdate;
    public static ConfigEntry<bool> AutoUpdate;

    // 模组下载链接
    public static string ModDownloadURL;

    public static void UpdateMod()
    {
    }
}