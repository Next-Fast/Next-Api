using System.Linq;
using NextShip.Utilities;
using UnityEngine;

namespace NextShip.Utils;

public static class AssetUtils
{
    private static CosmeticsCache _CosmeticsCache = ShipStatus.Instance.CosmeticsCache;
    private static HatManager _hatManager = FastDestroyableSingleton<HatManager>.Instance;

    public static PetBehaviour GetPetBehaviour(this string id)
    {
        var asset = _CosmeticsCache.GetPet(id);
        if (asset.Data.ProdId == "pet_EmptyPet" && _hatManager.allPets.FirstOrDefault(n => n.ProdId == id) != null)
        {
            var Asset = _hatManager.GetPetById(id).CreateAddressableAsset();
            Asset.LoadAsync((System.Action)(() => asset = Asset.GetAsset()));
        }
        return asset;
    }

    public static HatViewData GetHatViewData(this string id)
    {
        var asset = _CosmeticsCache.GetHat(id);
        return asset;
    }

    public static SkinViewData GetSkinViewData(this string id)
    {
        var asset = _CosmeticsCache.GetSkin(id);
        return asset;
    }

    public static VisorViewData GetVisorViewData(this string id)
    {
        var asset = _CosmeticsCache.GetVisor(id);
        return asset;
    }

    public static NamePlateViewData GetNamePlateViewData(this string id)
    {
        var asset = _CosmeticsCache.GetNameplate(id);
        return asset;
    }
}