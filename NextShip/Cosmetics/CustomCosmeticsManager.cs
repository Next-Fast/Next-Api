using System.Collections.Generic;

namespace NextShip;

public class CustomCosmeticsManager
{
    public static List<string> AllCustomCosemeticsId = new();
    public static Dictionary<string, CosemeticsRepoTyep> AllCosemeticsRepo = new Dictionary<string, CosemeticsRepoTyep>();

    public static string RepoFile = "CosemeticsRepo";

    public static void ReadRepoFile()
    {
        
    }
}

public enum CosemeticsRepoTyep
{
    TOR,
    EXR,
    NOS,
    TIS
}