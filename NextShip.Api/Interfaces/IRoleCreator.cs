namespace NextShip.Api.Interfaces;

public interface IRoleCreator
{
    public T Create<T>(IRole role) where T : RoleBase;
}