using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace TheIdealShip.Modules
{
    public static class ModUpdater
    {
        // HUdate=HasUpdate 判断是否有更新
        public static bool HUpdate;
        // 模组下载链接
        public const string ModDownloadURL = "";
        // 构建件存放
        public const string nightlyURL ="https://nightly.link/TheIdealShipAU/TheIdealShip/workflows/Build-Release/main";
        static string GithubURL = TheIdealShipPlugin.GithubURL;
        static string GiteeURL = TheIdealShipPlugin.GiteeURL;
        public static async void ap()
        {
            var http = new HttpClient();
            var langid = AmongUs.Data.Legacy.LegacySaveManager.lastLanguage;
            string UpdateURL = TheIdealShipPlugin.isCn ? GiteeURL : GithubURL;
            string apiURL = UpdateURL.Replace("https://", "https://api.");
            var GL = await http.GetAsync(apiURL + "/releases/latest/");
            string json = await GL.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(json);
            string tagname = data["tag_name"]?.ToString();
            Version Ver = Version.Parse(tagname.Replace("v", ""));
        }

        public static void UpdateMod()
        {
            
        }
    }
}