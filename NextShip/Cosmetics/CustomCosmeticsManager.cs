using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using HarmonyLib;
using NextShip.Config;
using NextShip.Cosmetics.Loaders;
using NextShip.Manager;
using UnityEngine;

namespace NextShip.Cosmetics;

public static class CustomCosmeticsManager
{
    public const string RepoFile = "CosmeticRepo";
    public static Dictionary<string, CosmeticsInfo> AllCustomCosmeticNameAndInfo = new();
    public static Dictionary<string, CosmeticRepoType> AllCosmeticRepoRepo = new();

    public static HashSet<Sprite> AllCustomCosmeticSprites = new();
    public static HashSet<string> AllCosmeticId = new();

    public static Dictionary<string, HatViewData> AllCustomHatViewData = new();
    public static List<SkinViewData> AllCustomSkinViewData = new();
    public static List<VisorViewData> AllCustomVisorViewData = new();
    public static List<NamePlateViewData> AllCustomNamePlateViewData = new();


    private static CosmeticsLoader _Loaders;
    public static List<CosmeticsLoader> AllLoaders = new();

    internal static readonly string RepoFilePath =
        Path.Combine(FilesManager.CreativityPath, RepoFile.Is(FilesManager.FileType.Json));

    private static readonly CosmeticsConfig[] ModConfig =
    {
    };

    public static Sprite GetSprite(string name)
    {
        return AllCustomCosmeticSprites.FirstOrDefault(n => n.name == name);
    }

    public static void LoadHat()
    {
        var configs = new List<CosmeticsConfig>();
        configs.AddRange(ModConfig);
        configs.AddRange(ReadRepoFile());
        if (configs.Count == 0) return;

        try
        {
            foreach (var config in from config in configs
                     let regex =
                         new Regex(
                             @"/https?\/\/:[-a-z0-9]+(\.[-a-z0-9])*\.(com|cn|edu|uk)\/[-a-z0-9_:@&?=+,.!/~*'%$]*/ig")
                     where regex.IsMatch(config.RepoURL)
                     select config)
            {
                Load(config.CosmeticRepoType);
                _ = _Loaders?.LoadFormOnRepo(config);
            }
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }

    public static List<CosmeticsConfig> ReadRepoFile()
    {
        using TextReader textReader = new StreamReader(RepoFilePath);
        return JsonSerializer.Deserialize<List<CosmeticsConfig>>(textReader.ReadToEnd());
    }

    public static void WriteRepo(List<CosmeticsConfig> repos)
    {
        WriteRepoFile(JsonSerializer.Serialize(repos));
    }

    public static void WriteRepoFile(string text)
    {
        using TextWriter textWriter = new StreamWriter(RepoFilePath);
        textWriter.Write(text);
    }

    private static void Load(CosmeticRepoType cosmeticRepoType)
    {
        _Loaders = cosmeticRepoType switch
        {
            CosmeticRepoType.TOR => new TORCosmeticsLoader(),
            CosmeticRepoType.EXR => null,
            CosmeticRepoType.NOS => null,
            CosmeticRepoType.TIS => null,
            _ => null
        };

        if (_Loaders != null)
            AllLoaders.Add(_Loaders);
    }

    public static void AddToList(HatManager __instance)
    {
        foreach (var vaLoader in AllLoaders)
        {
            if (vaLoader.AddToList) continue;
            var list = new List<HatData>();
            list.AddRange(__instance.allHats);
            vaLoader.Hats.Values.Do(n => n.Keys.Do(data => list.Add(data)));
            __instance.allHats = list.ToArray();
        }
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