using System;
using UnityEngine;

public class SimpleRoleInfo
{
    public Type ClassType;
    public Color color;
    public string name;
    public RoleBase roleBase;
    public RoleId roleId;
    public int roleIntId;
    public string RoleStringId;
    public RoleTeam roleTeam;
    public RoleType roleType;

    public SimpleRoleInfo
    (
        Type classType,
        RoleId id,
        Color color,
        RoleTeam team,
        RoleType type,
        string roleName = "",
        int roleIntId = -1
    )
    {
        ClassType = classType;
        name = roleName == "" ? roleName : id.ToString();
        roleId = id;
        roleTeam = team;
        roleType = type;
        this.roleIntId = roleIntId;
        this.color = color;
    }

    public SimpleRoleInfo
    (
        Type classType,
        Color color,
        RoleTeam team,
        RoleType type,
        string roleStringId = "",
        string roleName = "",
        int roleIntId = -1
    )
    {
        ClassType = classType;
        name = roleName == "" ? roleName : roleStringId;
        RoleStringId = roleStringId;
        roleTeam = team;
        roleType = type;
        this.roleIntId = roleIntId;
        this.color = color;
    }


    public bool CreateInstance(PlayerControl player)
    {
        try
        {
            var con = ClassType.GetConstructor(new[] { typeof(PlayerControl) });
            if (con == null) return false;

            roleBase = (RoleBase)con.Invoke(new object[] { player });
            roleBase.SimpleRoleInfo = this;
            Info($"创建角色实例成功: {name} : {player.name}");
            return true;
        }
        catch
        {
            Warn($"创建角色实例失败: {name}");
            return false;
        }
    }
}