using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;

namespace Next.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class Il2CppRegisterAttribute(params Type[] interfaces) : Attribute
{
    public Il2CppRegisterAttribute() : this(Type.EmptyTypes)
    {
    }


    public Type[] Interfaces { get; } = interfaces;

    public static void Registration(Type? type)
    {
        Info("[Il2CppRegister] Start Registration");

        var attribute =
            type?.GetCustomAttribute<Il2CppRegisterAttribute>();

        if (attribute != null) registrationForTarget(type, attribute.Interfaces);

        Info("[Il2CppRegister]Complete Registration");
    }

    private static void registrationForTarget(Type? targetType, Type[] interfaces)
    {
        var targetBase = targetType?.BaseType;

        Il2CppRegisterAttribute? baseAttribute = null;

        if (targetBase != null)
            baseAttribute = targetBase.GetCustomAttribute<Il2CppRegisterAttribute>();

        if (baseAttribute != null)
            registrationForTarget(targetBase, baseAttribute.Interfaces);

        Debug($"Registration {targetType}");

        if (ClassInjector.IsTypeRegisteredInIl2Cpp(targetType)) return;

        try
        {
            ClassInjector.RegisterTypeInIl2Cpp
            (
                targetType,
                new RegisterTypeOptions
                {
                    Interfaces = interfaces
                }
            );
        }
        catch (Exception e)
        {
            var excStr = targetType.FullDescription();
            Error($"Registion Fail!!    Target:{excStr}\n Il2CppError:{e}");
        }
    }
}