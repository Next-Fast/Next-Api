using NextShip.Cosmetics;

namespace NextShip.Config;


public class CosmeticsConfig
{
    public CosmeticRepoType CosmeticRepoType { get; set; }
    public CosmeticType CosmeticType { get; set; }
    public CosmeticLoadType CosmeticLoadType　{ get; set; }
    public string RepoName　{ get; set; }
    public string RepoURL　{ get; set; }
}