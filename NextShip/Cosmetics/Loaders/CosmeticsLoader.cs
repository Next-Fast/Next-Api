using System.Collections.Generic;
using System.Threading.Tasks;
using NextShip.Api.Config;
using NextShip.Api.Enums;
using UnityEngine;

namespace NextShip.Cosmetics.Loaders;

public abstract class CosmeticsLoader
{
    public List<CosmeticsInfo> AllCosmeticsInfo;
    public List<HatViewData> AllHat;
    public List<NamePlateViewData> AllNamePlate;
    public List<Sprite> AllSprite;


    public List<VisorViewData> AllVisor;

    public string HatJsonName = "CustomHats.json";
    public Dictionary<string, Dictionary<HatData, HatViewData>> Hats = new();

    protected CosmeticsLoader()
    {
        CustomCosmeticsManager.AllLoaders.Add(this);
        AllHat = new List<HatViewData>();
        AllVisor = new List<VisorViewData>();
        AllNamePlate = new List<NamePlateViewData>();
        AllSprite = new List<Sprite>();
        AllCosmeticsInfo = new List<CosmeticsInfo>();
    }

    public bool Loaded { get; protected set; }
    public bool AddToList { get; protected set; }

    public abstract CosmeticType GetCosmeticType();
    public abstract CosmeticRepoType GetCosmeticRepoType();

    public void Load()
    {
    }

    public virtual void Remove()
    {
    }

    public virtual void LoadFormDisk()
    {
    }

    public virtual void LoadFormResources()
    {
    }

    public virtual Task LoadFormOnRepo(CosmeticsConfig cosmeticsConfig)
    {
        return Task.CompletedTask;
    }
}

public record CosmeticsInfo
{
    public CosmeticType CosmeticType { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }


    public string Id { get; set; }
    public string Package { get; set; }
    public string Condition { get; set; }

    public string Resource { get; set; }
    public string FlipResource { get; set; }
    public string BackFlipResource { get; set; }
    public string BackResource { get; set; }
    public string ClimbResource { get; set; }

    public string ResHash { get; set; }
    public string ResHashBack { get; set; }
    public string ResHashClimb { get; set; }
    public string ResHashFlip { get; set; }
    public string ResHashBackFlip { get; set; }

    public bool Bounce { get; set; }
    public bool Adaptive { get; set; }
    public bool Behind { get; set; }
}