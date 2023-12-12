using System.Reflection;

namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ServiceAddAttribute : Attribute
{
    public static void Registration(IServiceProvider provider, Assembly assembly)
    {
        var Fields = assembly.GetFieldInfos();

        foreach (var VarField in Fields.Where(n => n.Is<ServiceAddAttribute>()))
        {
            if (!VarField.IsStatic) continue;

            var type = VarField.FieldType;
            var value = provider.GetService(type);
            VarField.SetValue(null, value);
        }
    }
}