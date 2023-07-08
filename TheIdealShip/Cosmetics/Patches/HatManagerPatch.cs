using System.Collections.Generic;
using HarmonyLib;

namespace TheIdealShip.Patches;

[HarmonyPatch(typeof(HatManagerPatch))]
public static class HatManagerPatch
{
    public static List<HatViewData> AllCacheHatViewDatas = new();
    public static List<NamePlateViewData> AllCacheNamePlateViewDatas = new();
    public static List<SkinViewData> AllCacheSkinViewDatas = new();
    public static List<VisorViewData> AllCacheVisorViewDatas = new();

    public static bool initialized;

    [HarmonyPatch(nameof(HatManager.Initialize))]
    [HarmonyPostfix]
    public static void InitHatCache(HatManager __instance)
    {
        __instance.allHats.Do(n => n.AddToChache());
        __instance.allSkins.Do(n => n.AddToChache());
        __instance.allVisors.Do(n => n.AddToChache());
        __instance.allNamePlates.Do(n => n.AddToChache());
        initialized = true;
    }

    public static void AddToChache(this HatData data)
    {
        var Asset = data.CreateAddressableAsset();
        if (!Asset.IsLoaded()) Asset.LoadAsync();
        AllCacheHatViewDatas.Add(Asset.GetAsset());
    }

    public static void AddToChache(this NamePlateData data)
    {
        var Asset = data.CreateAddressableAsset();
        if (!Asset.IsLoaded()) Asset.LoadAsync();
        AllCacheNamePlateViewDatas.Add(Asset.GetAsset());
    }

    public static void AddToChache(this VisorData data)
    {
        var Asset = data.CreateAddressableAsset();
        if (!Asset.IsLoaded()) Asset.LoadAsync();
        AllCacheVisorViewDatas.Add(Asset.GetAsset());
    }

    public static void AddToChache(this SkinData data)
    {
        var Asset = data.CreateAddressableAsset();
        if (!Asset.IsLoaded()) Asset.LoadAsync();
        AllCacheSkinViewDatas.Add(Asset.GetAsset());
    }
}