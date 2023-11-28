namespace NextShip.Roles;

public class Postman : Role
{
    private static readonly SimpleRoleInfo PostmanRoleInfo = new();


    public Postman()
    {
        CreateRoleBase = n => new PostmanBase(n);
        SimpleRoleInfo = PostmanRoleInfo;
    }

    private class PostmanBase : RoleBase
    {
        public PostmanBase(PlayerControl player) : base(player)
        {
        }
    }
}