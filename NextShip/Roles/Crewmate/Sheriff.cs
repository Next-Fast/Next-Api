using System;
using System.Collections.Generic;
using NextShip.Api.Interfaces;
using OtherAttribute;
using UnityEngine;

namespace NextShip.Roles.Crewmate;

[FastAddRole]
public class Sheriff : IRole
{
    public Type RoleBaseType { get; set; } = typeof(SheriffBase);
    
    public Type RoleType { get; set; } = typeof(Sheriff);
    public List<RoleBase> RoleBase { get; set; } = [];
    public List<PlayerControl> Players { get; set; } = [];
    public bool EnableAssign { get; set; } = true;
    
    public Func<PlayerControl, RoleBase> CreateRoleBase { get; set; }

    public SimpleRoleInfo SimpleRoleInfo { get; set; } = new()
    {
        RoleColor = Color.yellow,
        roleTeam = RoleTeam.Crewmate,
        roleType = Api.Roles.RoleType.MainRole,
        Name = nameof(Sheriff),
        RoleStringId = nameof(Sheriff)
    };
    
    public bool CanCreate(IRole role, PlayerControl player)
    {
        return true;
    }

    public class SheriffBase(PlayerControl player) : RoleBase(player)
    {
        
    }

    public void Dispose()
    {
    }
}