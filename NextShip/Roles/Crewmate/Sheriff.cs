using System;
using System.Collections.Generic;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;
using NextShip.Manager;
using UnityEngine;

namespace NextShip.Roles.Crewmate;

[FastAddRole]
public class Sheriff : IRole
{
    public List<OptionBase> AllOption = [];

    public Sheriff()
    {
        var manager = Main._Service.Get<NextOptionManager>();
    }

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

    public void OptionCreate(INextOptionManager _nextOptionManager)
    {
    }

    public void Dispose()
    {
    }
}

public class SheriffBase : RoleBase
{
    public SheriffBase(PlayerControl player) : base(player)
    {
        _eventManager.RegisterListener(new EventListener());
    }

    private class EventListener : IEventListener
    {
    }
}