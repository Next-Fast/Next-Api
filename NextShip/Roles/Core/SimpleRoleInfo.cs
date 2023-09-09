using System;
using System.Collections.Generic;
using Il2CppSystem.Linq.Expressions;
using NextShip.Options;
using UnityEngine;

namespace NextShip.Roles.Core;

public class SimpleRoleInfo
{
    public Color RoleColor;
    public string Name;
    public RoleId roleId;
    public int roleIntId;
    public string RoleStringId;
    public RoleTeam roleTeam;
    public RoleType roleType;
    public Role ParentRole { get; private set; }
    
    
    public SimpleRoleInfo
    (
        RoleId id,
        Color color,
        RoleTeam team,
        RoleType type,
        string roleStringId,
        string roleName,
        int roleIntId)
    {
        RoleColor = color;
        Name = roleName;
        roleId = id;
        this.roleIntId = roleIntId;
        RoleStringId = roleStringId;
        roleTeam = team;
        roleType = type;
        
        // Add to RoleManager        
        
        RoleManager.Get().AllSimpleRoleInfos.Add(this);
    }
    
    public SimpleRoleInfo
    (
        RoleId id,
        Color color,
        RoleTeam team,
        RoleType type,
        string roleName) : this(id,color, team, type, "", roleName, -1)
    {
    }

    public SimpleRoleInfo
    (
        Type classType,
        Color color,
        RoleTeam team,
        RoleType type,
        string roleName
    ) : this(RoleId.none, color, team, type, "", roleName, -1)
    {
    }
    
}