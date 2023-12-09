namespace NextShip.Api.Interfaces;

public interface IRoleManager
{
    public void Register(IRole role);

    public void UnRegister(IRole role);

    public void AddCreator(IRoleCreator creator);

    public void RemoveCreator(IRoleCreator creator);
}