using UnityEngine;
using System;

public class SimpleRoleInfo
{
    public Type ClassType;
    public string name;
    public RoleBase roleBase;
    public RoleId roleId;
    public RoleTeam roleTeam;
    public RoleType roleType;
    public Color color;
    public SimpleRoleInfo
    (
        RoleId id,
        Color color,
        RoleTeam team,
        RoleType type
    )
    {
        this.name = id.ToString();
        this.roleId = id;
        this.color = color;
    }

    public  void setBase(RoleBase roleBase) => this.roleBase = roleBase;
}