using System;
using UnityEngine;

public class SimpleRoleInfo
{
    public Type ClassType;
    public Color color;
    public string name;
    public RoleBase roleBase;
    public RoleId roleId;
    public RoleTeam roleTeam;
    public RoleType roleType;

    public SimpleRoleInfo
    (
        RoleId id,
        Color color,
        RoleTeam team,
        RoleType type
    )
    {
        name = id.ToString();
        roleId = id;
        this.color = color;
    }

    public void setBase(RoleBase roleBase)
    {
        this.roleBase = roleBase;
    }
}