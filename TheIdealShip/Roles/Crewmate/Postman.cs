using System.Collections.Generic;

namespace TheIdealShip.Roles;

public class Postman : RoleBase
{
    public static SimpleRoleInfo simpleRoleInfo = new
    (
        RoleId.Postman,
        UnityEngine.Color.blue,
        RoleTeam.Crewmate,
        RoleType.MainRole
    );

    public Postman(PlayerControl player) : base(player)
    {
        setInfo(simpleRoleInfo);
    }

    public void OptionLoad()
    {
    }
}
