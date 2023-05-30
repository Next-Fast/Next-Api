global using TheIdealShip.Updates;

using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace TheIdealShip.Updates
{
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
}