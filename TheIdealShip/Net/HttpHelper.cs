using System.IO;
using System.Net.Http;

namespace TheIdealShip;

public class HttpHelper
{
    // https://github.com/KARPED1EM/TownOfHostEdited/blob/TOHE/Modules/ModUpdater.cs
    public static string Get(string url)
    {
        string result = "";
        HttpClient req = new HttpClient();
        var res = req.GetAsync(url).Result;
        Stream stream = res.Content.ReadAsStreamAsync().Result;

        try
        {
            //获取内容
            using StreamReader reader = new(stream);
            result = reader.ReadToEnd();
        }
        catch
        {
            log.Error("读取失败","Http-Get");
        }

        finally
        {
            stream.Close();
        }

        return result;
    }
}