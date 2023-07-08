using TheIdealShip.Utilities;
using UnityEngine;

namespace TheIdealShip.Utils;

public static class AssetUtils
{
    public static PetBehaviour GetPetBehaviourById(this string id)
    {
        var Asset = FastDestroyableSingleton<HatManager>.Instance.GetPetById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static HatViewData GetHatViewDataById(this string id)
    {
        var Asset = FastDestroyableSingleton<HatManager>.Instance.GetHatById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static SkinViewData GetSkinViewDataById(this string id)
    {
        var Asset = FastDestroyableSingleton<HatManager>.Instance.GetSkinById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static VisorViewData GetVisorViewDataById(this string id)
    {
        var Asset = FastDestroyableSingleton<HatManager>.Instance.GetVisorById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static NamePlateViewData GetNamePlateViewDataById(this string id)
    {
        var Asset = FastDestroyableSingleton<HatManager>.Instance.GetNamePlateById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static Sprite GetSprite(this SkinViewData viewData)
    {
        return viewData.GetSprite();
    }

    public static Sprite GetSprite(this PetBehaviour viewData)
    {
        return viewData.GetSprite();
    }

    public static Sprite GetSprite(this HatViewData viewData)
    {
        return viewData.GetSprite();
    }

    public static Sprite GetSprite(this NamePlateViewData viewData)
    {
        return viewData.GetSprite();
    }

    public static Sprite GetSprite(this VisorViewData viewData)
    {
        return viewData.GetSprite();
    }
}