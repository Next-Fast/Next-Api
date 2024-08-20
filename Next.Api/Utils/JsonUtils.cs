using Newtonsoft.Json.Linq;

namespace Next.Api.Utils;

public static class JsonUtils
{
    public static string? GetString(this JToken token, string key)
    {
        return token[key]?.ToString();
    }
}