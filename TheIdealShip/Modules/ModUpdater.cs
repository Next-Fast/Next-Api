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
        public static string ModDownloadURL = "";
        public static async void ap()
        {
            var http = new HttpClient();
            var GithubURL = TheIdealShipPlugin.GithubURL;
            string apiURL = GithubURL.Replace("github.com", "api.github.com");
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