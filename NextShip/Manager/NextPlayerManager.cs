using System.Collections.Generic;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class NextPlayerManager : IPlayerManager
{
    private static NextPlayerManager? _nextPlayerManager;

    private readonly List<NextInfo> PlayerInfos = new();

    public static NextPlayerManager Instance => _nextPlayerManager ??= new NextPlayerManager();

    public NextInfo GetPlayerInfo(PlayerControl player)
    {
        return PlayerInfos.Find(n => n.PlayerControl == player)!;
    }

    public void CreatePlayerInfo()
    {
    }
}