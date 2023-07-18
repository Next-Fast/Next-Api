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
    }
    
    [OptionLoad]
    public static void OptionLoad()
    {
    }
}