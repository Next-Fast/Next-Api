using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextShip.Config;
using UnityEngine;

namespace NextShip.Cosmetics.Loaders;

public abstract class CosmeticsLoader
{
    protected CosmeticsLoader()
    {
        CustomCosmeticsManager.AllLoaders.Add(this);
        AllHat = new List<HatViewData>();
        AllVisor = new List<VisorViewData>();
        AllNamePlate = new List<NamePlateViewData>();
        AllSprite = new List<Sprite>();
        AllCosmeticsInfo = new List<CosmeticsInfo>();
    }

    public string HatJsonName = "CustomHats.json";

    public abstract CosmeticType GetCosmeticType();
    public abstract CosmeticRepoType GetCosmeticRepoType();

    public bool Loaded { get; protected set; }
    public bool AddToList { get; protected set; }

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
    

    public List<VisorViewData> AllVisor;
    public List<NamePlateViewData> AllNamePlate;
    public List<HatViewData> AllHat;
    public List<Sprite> AllSprite;
    public List<CosmeticsInfo> AllCosmeticsInfo;
    public Dictionary<string, Dictionary<HatData, HatViewData>> Hats = new ();
}

public record CosmeticsInfo
{    
    public CosmeticType CosmeticType { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    
    
    public string Id { get; set; }
    public string Package { get; set;}
    public string Condition { get; set;}
    
    public string Resource { get; set;}
    public string FlipResource { get; set;}
    public string BackFlipResource { get; set;}
    public string BackResource { get; set;}
    public string ClimbResource { get; set;}
    
    public string ResHash { get; set;}
    public string ResHashBack { get; set;}
    public string ResHashClimb { get; set;}
    public string ResHashFlip { get; set;}
    public string ResHashBackFlip { get; set;}    
    
    public bool Bounce { get; set;}
    public bool Adaptive { get; set;}
    public bool Behind { get; set;}
}