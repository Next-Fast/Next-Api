using UnityEngine;

namespace NextShip.Api.Roles;

public sealed class SimpleRoleInfo(RoleId id,
    Color color,
    RoleTeam team,
    RoleType type,
    string roleStringId,
    string roleName,
    int roleIntId)
{
    public string Name = roleName;
    public Color RoleColor = color;
    public RoleId roleId = id;
    public int roleIntId = roleIntId;
    public string RoleStringId = roleStringId;
    public RoleTeam roleTeam = team;
    public RoleType roleType = type;

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
}