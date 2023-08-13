using System;
using System.Collections.Generic;
using HarmonyLib;

namespace NextShip.Patches;

[HarmonyPatch(typeof(HatManager))]
public static class HatManagerPatch
{
    private static bool initialized = true;

    [HarmonyPatch(nameof(HatManager.Initialize))]
    [HarmonyPostfix]
    public static void InitHatCache(HatManager __instance)
    {
        if (initialized) return;

        TaskUtils.StartTask(() => initialized = AllCosmeticsCache.StartCache(__instance));
    }
}

public static class AllCosmeticsCache
{
    public static List<HatViewData> AllHatViewDatasCache = new();
    public static List<NamePlateViewData> AllNamePlateViewDatasCache = new();
    public static List<SkinViewData> AllSkinViewDatasCache = new();
    public static List<VisorViewData> AllVisorViewDatasCache = new();
    public static List<PetBehaviour> AllPetBehavioursCache = new();


    public static bool StartCache(HatManager __instance)
    {
        try
        {
            __instance.allHats.Do(n => n.AddToCache());
            __instance.allSkins.Do(n => n.AddToCache());
            __instance.allVisors.Do(n => n.AddToCache());
            __instance.allNamePlates.Do(n => n.AddToCache());
            __instance.allPets.Do(n => n.AddToCache());
            Info("缓存成功");
            return true;
        }
        catch (Exception e)
        {
            Exception(e);
            Error("缓存失败");
            return false;
        }
    }

    private static void AddToCache(this HatData data)
    {
        var Asset = data.CreateAddressableAsset();
        Asset.LoadAsync();
        AllHatViewDatasCache.Add(Asset.GetAsset());
    }

    private static void AddToCache(this NamePlateData data)
    {
        var Asset = data.CreateAddressableAsset();
        Asset.LoadAsync();
        AllNamePlateViewDatasCache.Add(Asset.GetAsset());
    }

    private static void AddToCache(this VisorData data)
    {
        var Asset = data.CreateAddressableAsset();
        Asset.LoadAsync();
        AllVisorViewDatasCache.Add(Asset.GetAsset());
    }

    private static void AddToCache(this SkinData data)
    {
        var Asset = data.CreateAddressableAsset();
        Asset.LoadAsync();
        AllSkinViewDatasCache.Add(Asset.GetAsset());
    }

    private static void AddToCache(this PetData data)
    {
        var Asset = data.CreateAddressableAsset();
        Asset.LoadAsync();
        AllPetBehavioursCache.Add(Asset.GetAsset());
    }
}