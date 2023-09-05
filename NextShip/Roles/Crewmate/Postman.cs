using NextShip.Utilities.Attributes;
using UnityEngine;

namespace NextShip.Roles;

public class Postman : Role
{
    public static SimpleRoleInfo PostmanRoleInfo;
    
    
    public Postman()
    {
        CreateRoleBase = n => new PostmanBase(n);
        SimpleRoleInfo = PostmanRoleInfo;
    }
    
    public class PostmanBase : RoleBase
    {
        public PostmanBase(PlayerControl player) : base(player)
        {
        }
    }
}