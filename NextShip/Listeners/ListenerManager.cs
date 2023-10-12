using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using NextShip.Game.GameEvents;

namespace NextShip.Listeners;

public class ListenerManager
{
    private static ListenerManager _ListenerManager;
    public readonly List<IGameEvent> allGameEvents = new();
    private readonly List<MethodInfo> allMethodInfos = new();

    private ListenerManager()
    {
        _ListenerManager = this;
    }

    internal void RegisterMethod(MethodInfo methodInfo)
    {
        allMethodInfos.Add(methodInfo);
    }

    internal void RegisterGameEvent(IGameEvent gameEvent)
    {
        allGameEvents.Add(gameEvent);
    }
    
    internal void URegisterGameEvent(IGameEvent gameEvent)
    {
        allGameEvents.Remove(gameEvent);
    }

    internal bool Start(string name, object Target, params object[] objects)
    {
        var method = allMethodInfos.Find(n => n.Name == name);
        if (method == null) return false;

        var list = new List<MethodInfo>();
        foreach (var varMethod in allMethodInfos.Where(n =>
                     n.Name == name && n.GetGenericArguments().Contains((Type[])objects))) list.Add(varMethod);

        if (list.Count == 0) return false;

        try
        {
            list.Do(n => n.Invoke(Target, objects));
            return true;
        }
        catch (Exception e)
        {
            Exception(e);
            return false;
        }
    }

    public static ListenerManager Get()
    {
        return _ListenerManager ?? new ListenerManager();
    }
}