using System.Security.Cryptography;

namespace Next.Api.Utils;

public class HashUtils
{
    public static string GetFileMD5Hash(string path)
    {
        var hash = MD5.Create();
        using var stream = File.OpenRead(path);
        var hashBytes = hash.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public static string GetFileSHA256Hash(string path)
    {
        var hash = SHA256.Create();
        using var stream = File.OpenRead(path);
        var hashBytes = hash.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}