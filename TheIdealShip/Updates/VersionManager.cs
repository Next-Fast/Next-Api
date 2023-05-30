namespace TheIdealShip.Updates;

public static class VersionManager
{
    // HUdate=HasUpdate 判断是否有更新
    public static bool HUpdate;
    // 构建件存放
    public const string nightlyURL = "https://nightly.link/TheIdealShipAU/TheIdealShip/workflows/Build-Release/main";
    // 服务器alist
    public const string alistURL = "http://pan.pafyx.top/TIS";
    // 2018k.cnAPI
    public const string KApiURL = "http://api.2018k.cn";
    public const string KApiId = "FC912F87DE524E5393F6F35B66B8ACEB";

    static string GithubURL = TheIdealShipPlugin.GithubURL;
    static string GiteeURL = TheIdealShipPlugin.GiteeURL;

    private static string KApi_addId(this string URL) => URL + "?id=" + KApiId;
    private static string KApi_addCheckVersion(this string URL) => URL +  "/checkVersion";
    private static string KApi_addGetInfo(this string URL) => URL + "/getExample";

    public static void VersionCheck()
    {
        
    }

    // 使用Github检查更新
    public static bool GithubVersionCheck(string URL)
    {
        return false;
    }

    // 使用Gitee检测更新
    public static bool GiteeVersionCheck(string URL)
    {
        return false;
    }

    // 使用2018k.cn检测更新
    public static bool KVersionCheck(string URL)
    {
        return false;
    }
}