using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace NextShip.Utilities.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class InitAttribute : Attribute
{
    public static List<MethodInfo> MethodInfos = new List<MethodInfo>();
    
    internal static void Registration(Type type)
    {
        Info("Start Registration", filename: MethodUtils.GetClassName());
        var ConstructorS = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
        ConstructorS.Do(Add);
        
        static void Add (MethodInfo methodInfo)
        {
            if (methodInfo.GetCustomAttribute<InitAttribute>() == null) return;
            MethodInfos.Add(methodInfo);
            Debug($"Add {methodInfo.Name}");
        }
        
        Info($"Statically Initialized Class {type}", filename: MethodUtils.GetClassName());
    }

    public static void Start()
    {
        MethodInfos.Do(n => n.Invoke(null,null));
    }
}