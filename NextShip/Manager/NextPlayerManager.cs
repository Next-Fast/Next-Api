using System.Collections.Generic;
using System.Linq;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class NextPlayerManager : IPlayerManager
{
    private readonly List<NextInfo> PlayerInfos = new();


    public static NextPlayerManager Instance => Main._Service.Get<NextPlayerManager>();

    public NextInfo GetPlayerInfo(PlayerControl player)
    {
        return PlayerInfos.Find(n => n.PlayerControl == player)!;
    }

    public void CreatePlayerInfo()
    {
    }

    public bool TryGetPlayer(PlayerControl player, out NextInfo? info)
    {
        if (PlayerInfos.Exists(n => n.PlayerControl == player))
        {
            info = PlayerInfos.First(n => n.PlayerControl == player);
            return true;
        }

        info = null;
        return false;
    }


    public void Clear()
    {
        PlayerInfos.Clear();
    }
}