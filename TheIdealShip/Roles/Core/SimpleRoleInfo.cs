using System;

public class SimpleRoleInfo
{
    public Type ClassType;
    public Func<PlayerControl, RoleBase> CreateInstance;
    public RoleId roleId;
    public RoleTeam roleTeam;
    public RoleType roleType;
    public SimpleRoleInfo
    (

    )
    {
    }
}