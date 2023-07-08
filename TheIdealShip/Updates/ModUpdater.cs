global using TheIdealShip.Updates;

namespace TheIdealShip.Updates;

public static class ModUpdater
{
    // HUdate=HasUpdate 判断是否有更新
    public static bool HUpdate = VersionManager.HUpdate;

    // 模组下载链接
    public static string ModDownloadURL;

    public static void UpdateMod()
    {
    }
}