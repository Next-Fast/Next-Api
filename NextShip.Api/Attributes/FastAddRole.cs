using System.Reflection;
using NextShip.Api.Interfaces;

namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor)]
public class FastAddRole : Attribute
{
    public static void Registration(IRoleManager _roleManager, Assembly assembly)
    {
        var Types = assembly.GetTypes();

        foreach (var VarType in Types.Where(n => n.Is<FastAddRole>() && n.GetInterface(nameof(IRole)) != null))
        {
            var constructorInfos = VarType.GetConstructors().Where(n => n.Is<FastAddRole>());
            foreach (var variableConstructorInfo in constructorInfos)
            {
                _roleManager.Register(variableConstructorInfo.Invoke(null, null) as IRole);
            }
        }
    }
}