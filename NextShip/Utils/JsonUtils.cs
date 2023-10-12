using Newtonsoft.Json.Linq;
using NJson = Newtonsoft.Json;
using SJson = System.Text.Json;

namespace NextShip.Utils;

public static class JsonUtils
{
    public static string GetString(this JToken token, string key)
    {
        return token[key]?.ToString();
    }
}