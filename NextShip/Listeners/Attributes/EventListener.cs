using System;
using System.Reflection;
using NextShip.Game.GameEvents;

namespace NextShip.Listeners.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class EventListener : Attribute
{
    public static void Registration(Type type)
    {
        var methodInfos = type.GetMethods();
        foreach (var MethodInfo in methodInfos)
        {
            if (MethodInfo.GetCustomAttribute<EventListener>() == null) continue;
            if (MethodInfo.ReturnType == typeof(IGameEvent))
            {
                ListenerManager.Get().RegisterGameEvent(MethodInfo.Invoke(null, null) as IGameEvent);
                continue;
            }

            if (!MethodInfo.Name.Contains("On")) continue;

            ListenerManager.Get().RegisterMethod(MethodInfo);
        }

        if (type.GetCustomAttribute<EventListener>() == null) return;
        foreach (var variableInterface in type.GetInterfaces())
        {
            if (variableInterface != typeof(IGameEvent)) continue;

            var gameEvent = type.Assembly.CreateInstance(type.FullName!) as IGameEvent;
            ListenerManager.Get().RegisterGameEvent(gameEvent);
        }
    }
}