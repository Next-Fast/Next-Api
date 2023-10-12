using System;
using System.Collections.Generic;
using System.Linq;
using NextShip.Game.GameEvents;
using NextShip.Listeners;
using NextShip.Listeners.Attributes;
using NextShip.Utilities;

namespace NextShip.Roles;

public partial class RoleManager
{
    private RoleAssigner Assigner => RoleAssigner.Get();
    
    public IEnumerator<RoleBase> GetAssignRole(PlayerControl player)
    {
        Assign: Assigner.AssignRole();
        
        if (!Assigner.Get(player, out var role))
            goto Assign;
        
        yield return role;
    }
}


public class RoleAssigner
{
    private static RoleAssigner Instance;

    public static RoleAssigner Get() => Instance ??= new RoleAssigner();

    public List<RoleBase> AllAssigns { private set; get; } = new ();

    private Random Random = new ();

    public bool Get(PlayerControl player, out RoleBase role)
    {
        role = AllAssigns
            .FirstOrDefault(n => n.Player == player);
        return role != null;
    }

    public void AssignRole()
    {
        var Roles = RoleManager.Get().Roles.Where(n => n.EnableAssign).ToList();

        foreach (var @base in from player in CachedPlayer.AllPlayers 
                 where 
                     player.CanAssaign() 
                     &&
                     Get(player, out _) let role = Roles[GetValue()] 
                 select 
                     role.CreateRoleBase(player))
        {
            if (@base.GetGameEvent() != null)
                ListenerManager.Get().RegisterGameEvent(@base.GetGameEvent());
                
            AllAssigns.Add(@base);
        }

        return;

        int GetValue() => Random.Next(0, Roles.Count - 1);
    }

    public void Restore()
    {
        foreach (var Base in AllAssigns)
        {
            ListenerManager.Get().URegisterGameEvent(Base.GetGameEvent());
        }
        
        
        AllAssigns = new List<RoleBase>();
        Random = new Random();
    }
}