using NextShip.Options;
using NextShip.Utilities.Attributes;
using UnityEngine;

namespace NextShip.Roles;

public class Postman : RoleBase
{
    public static SimpleRoleInfo simpleRoleInfo = new
    (
        typeof(Postman),
        RoleId.Postman,
        Color.blue,
        RoleTeam.Crewmate,
        RoleType.MainRole
    );

    public Postman(PlayerControl player) : base(player)
    {
        SimpleRoleInfo = simpleRoleInfo;
    }
    
    [OptionLoad]
    public static void OptionLoad()
    {
        RoleOptionBase Postman = new RoleOptionBase(simpleRoleInfo.name, -1, optionTab.Crewmate);
    }
}