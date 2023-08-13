using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace NextShip.Listeners;

[AttributeUsage(AttributeTargets.Method)]
public class GameEventListener : Attribute
{
    public static List<MethodInfo> AllEventMethodInfos = new();

    public static void Init(Type type)
    {
        AllEventMethodInfos = type.GetMethods().Where(n => n.GetCustomAttribute<GameEventListener>() != null).ToList();
    }

    public static void Start(string methodName, params object[] objects)
    {
        var methodInfos = AllEventMethodInfos.Where(n => n.Name == methodName).ToList();
        methodInfos.Do(n => n.Invoke(null, objects));
    }
}