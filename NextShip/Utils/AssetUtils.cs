using Il2CppSystem;
using NextShip.Utilities;
using UnityEngine;

namespace NextShip.Utils;

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
}