using System;
using Innersloth.Assets;
using TheIdealShip.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheIdealShip.Utils;

public static class AssetUtils
{
    public static PetBehaviour GetPetBehaviourById(this string id)
    {
        AddressableAsset<PetBehaviour> Asset = FastDestroyableSingleton<HatManager>.Instance.GetPetById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static HatViewData GetHatViewDataById(this string id)
    {
        AddressableAsset<HatViewData> Asset = FastDestroyableSingleton<HatManager>.Instance.GetHatById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static SkinViewData GetSkinViewDataById(this string id)
    {
        AddressableAsset<SkinViewData> Asset = FastDestroyableSingleton<HatManager>.Instance.GetSkinById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static VisorViewData GetVisorViewDataById(this string id)
    {
        AddressableAsset<VisorViewData> Asset = FastDestroyableSingleton<HatManager>.Instance.GetVisorById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static NamePlateViewData GetNamePlateViewDataById(this string id)
    {
        AddressableAsset<NamePlateViewData> Asset = FastDestroyableSingleton<HatManager>.Instance.GetNamePlateById(id).CreateAddressableAsset();
        Asset.LoadAsync();
        return Asset.GetAsset();
    }

    public static Sprite GetSprite(this SkinViewData viewData) => viewData.GetSprite();

    public static Sprite GetSprite(this PetBehaviour viewData) => viewData.GetSprite();

    public static Sprite GetSprite(this HatViewData viewData) => viewData.GetSprite();

    public static Sprite GetSprite(this NamePlateViewData viewData) => viewData.GetSprite();
    public static Sprite GetSprite(this VisorViewData viewData) => viewData.GetSprite();

}