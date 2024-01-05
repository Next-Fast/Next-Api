using System;
using System.Collections.Generic;
using System.Linq;
using NextShip.Api.Interfaces;
using NextShip.Manager;

namespace NextShip.Roles;

public class FastCreator : IRoleCreator
{
    public readonly List<IRole> AllRole = [];

    public void Dispose()
    {
        Clear();
    }

    public T Create<T>(IRole role) where T : RoleBase
    {
        return (T)role.RoleBase[0];
    }

    public T GetRole<T>(PlayerControl player) where T : class
    {
        return (AllRole.FirstOrDefault(n => n.Players.Contains(player)) as T)!;
    }

    public void Clear()
    {
        AllRole.Clear();
    }

    public IRole GetAssign()
    {
        var random = new Random();
        var list = Main._Service.Get<NextRoleManager>().Roles.Where(n => n.EnableAssign).ToList();
        return list[random.Next(list.Count)];
    }
}