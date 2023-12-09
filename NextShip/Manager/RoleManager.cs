using System.Collections.Generic;
using System.Linq;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public sealed class NextRoleManager : IRoleManager
{
    public readonly HashSet<IRoleCreator> RoleCreators = new();
    public readonly List<IRole> Roles = new();


    public void Register(IRole role)
    {
        Roles.Add(role);
    }

    public void UnRegister(IRole role)
    {
        Roles.Remove(role);
    }

    public void AddCreator(IRoleCreator creator)
    {
        RoleCreators.Add(creator);
    }

    public void RemoveCreator(IRoleCreator creator)
    {
        RoleCreators.Remove(creator);
    }

    public T GetRole<T>() where T : IRole
    {
        return (T)Roles.FirstOrDefault(n => n is T);
    }

    public IEnumerable<T> GetRoles<T>() where T : IRole
    {
        return Roles.FindAll(n => n is T).Select(n =>　(T)n);
    }

    public T GetCreator<T>() where T : IRoleCreator
    {
        return (T)RoleCreators.FirstOrDefault(n => n is T);
    }
}