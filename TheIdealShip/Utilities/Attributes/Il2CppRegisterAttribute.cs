using System.Reflection;
using System;
using Il2CppInterop.Runtime.Injection;
using HarmonyLib;

namespace TheIdealShip.Utilities.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class Il2CppRegisterAttribute : Attribute
{
    public Type[] Interfaces { get; private set; }

    public Il2CppRegisterAttribute()
    {
        this.Interfaces = Type.EmptyTypes;
    }

    public static void Registration(Type type)
    {
        log.Info("Start Registration","Il2CppRegister");

            Il2CppRegisterAttribute attribute =
                CustomAttributeExtensions.GetCustomAttribute<Il2CppRegisterAttribute>(type);
            if (attribute != null)
            {
                registrationForTarget(type, attribute.Interfaces);
            }

        log.Info("Complete Registration", "Il2CppRegister");
    }

    private static void registrationForTarget(Type targetType, Type[] interfaces)
    {
        Type targetBase = targetType.BaseType;

        Il2CppRegisterAttribute baseAttribute = null;

        if (targetType != null)
        {
            baseAttribute = CustomAttributeExtensions.GetCustomAttribute<Il2CppRegisterAttribute>(targetBase);
        }

        if (baseAttribute != null)
        {
            registrationForTarget(targetBase, baseAttribute.Interfaces);
        }

        log.Info($"Registration {targetType}", "Register");

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
            string excStr = GeneralExtensions.FullDescription(targetType);
            log.Error($"Registion Fail!!    Target:{excStr}\n Il2CppError:{e}", "Register");
        }

    }

}