using System;
using System.Reflection;

namespace TheIdealShip.Utilities.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LoadAttribute : Attribute
{
    internal static void Registration(Type type)
    {
        Info("Start Registration",filename: MethodUtils.GetClassName());
        if (type.GetCustomAttribute<LoadAttribute>() == null) return;
        ConstructorInfo constructor;
        if ((constructor = type.GetConstructor
            (
                BindingFlags.Public | 
                         BindingFlags.Static | 
                         BindingFlags.NonPublic,
                    Array.Empty<Type>()
                ))
            == null) return;
        
        constructor.Invoke(null, null);
        Info($"Statically Initialized Class {type}", "LoadAttribute");
    }
}