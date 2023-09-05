using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NextShip.Config;
using NextShip.Cosmetics.Loaders;
using NextShip.Manager;

namespace NextShip.Cosmetics;

public class CustomCosmeticsManager
{
    public static List<string> AllCustomCosmeticId = new();
    public static Dictionary<string, CosmeticRepoType> AllCosmeticRepoRepo = new ();

    public static CosmeticsLoader _Loaders;
    public static List<CosmeticsLoader> AllLoaders = new();

    public const string RepoFile = "CosmeticRepo";
    public static readonly string RepoFilePath = Path.Combine(FilesManager.CreativityPath, RepoFile.Is(FilesManager.FileType.Json));

    public static List<CosmeticsConfig> ReadRepoFile()
    {
        using TextReader textReader = new StreamReader(RepoFilePath);
        return JsonSerializer.Deserialize<List<CosmeticsConfig>>(textReader.ReadToEnd());
    }

    public static void WriteRepo(List<CosmeticsConfig> repos) => WriteRepoFile(JsonSerializer.Serialize(repos));
    
    public static void WriteRepoFile(string text)
    {
        using TextWriter textWriter = new StreamWriter(RepoFilePath);
        textWriter.Write(text);
    }

    public static void Load(CosmeticRepoType cosmeticRepoType)
    {
        _Loaders = cosmeticRepoType switch
        {
            CosmeticRepoType.TOR => null,
            CosmeticRepoType.EXR => null,
            CosmeticRepoType.NOS => null,
            CosmeticRepoType.TIS => null,
            _ => null
        };
    }
}

public enum CosmeticRepoType
{
    TOR,
    EXR,
    NOS,
    TIS
}

public enum CosmeticType
{
    Hat,
    NamePlate,
    Visor,
    Skin
}

public enum CosmeticLoadType
{
    Repo,
    Disk,
    Resources
}