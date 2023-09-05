using System.Collections.Generic;
using UnityEngine;

namespace NextShip.Cosmetics.Loaders;

public abstract class CosmeticsLoader
{
    protected CosmeticsLoader() { CustomCosmeticsManager.AllLoaders.Add(this); }
    
    public abstract CosmeticType GetCosmeticType();
    public abstract CosmeticRepoType GetCosmeticRepoType();

    public virtual void Load() {}

    public virtual void Remove() {}
    
    public virtual void LoadFormDisk() {}
    
    public virtual void LoadFormResources() {}
    
    public virtual void LoadFormOnRepo() {}
    
    


    public List<VisorViewData> AllVisor;
    public List<NamePlateViewData> AllNamePlate;
    public List<HatViewData> AllHat;
    public List<Sprite> AllSprite;
}