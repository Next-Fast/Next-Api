namespace Next.Api.Utils;

public class HttpUtils
{
    // https://github.com/KARPED1EM/TownOfHostEdited/blob/TOHE/Modules/ModUpdater.cs
    public static string Get(string url)
    {
        var result = "";
        var req = new HttpClient();
        var res = req.GetAsync(url).Result;
        var stream = res.Content.ReadAsStreamAsync().Result;

        try
        {
            //获取内容
            using StreamReader reader = new(stream);
            result = reader.ReadToEnd();
        }
        catch
        {
            Error("Http-Get 读取失败");
        }

        finally
        {
            stream.Close();
        }

        return result;
    }
}