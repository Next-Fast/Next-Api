using UnityEngine;

namespace NextShip.Api.Roles;

public sealed class SimpleRoleInfo
{
    public string Name;
    public Color RoleColor;
    public RoleId roleId;
    public int roleIntId;
    public string RoleStringId;
    public RoleTeam roleTeam;
    public RoleType roleType;

    public SimpleRoleInfo()
    {
    }

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
        string roleName) : this(id, color, team, type, "", roleName, -1)
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

    public Role ParentRole { get; }
}