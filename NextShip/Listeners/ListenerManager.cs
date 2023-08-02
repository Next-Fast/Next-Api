using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using NextShip.Game.GameEvents;

namespace NextShip.Listeners;

public class ListenerManager
{
    public static ListenerManager _ListenerManager;
    public List<IGameEvent> _GameEvents;
    public List<IRoleEvent> _RoleEvents;

    public ListenerManager()
    {
        _ListenerManager = this;
        Init();
    }

    public void Init()
    {
        _GameEvents = new List<IGameEvent>();
        _RoleEvents = new List<IRoleEvent>();
    }

    public void add(IRoleEvent roleEvent)
    {
        _RoleEvents.Add(roleEvent);
    }

    public void remove(IRoleEvent roleEvent)
    {
        _RoleEvents.Remove(roleEvent);
    }
    
    public void add(IGameEvent gameEvent)
    {
        _GameEvents.Add(gameEvent);
    }

    public void remove(IGameEvent gameEvent)
    {
        _GameEvents.Add(gameEvent);
    }

    public List<IRoleEvent> GetRoleEvents()
    {
        return _RoleEvents;
    }

    public List<IGameEvent> GetGameEvents()
    {
        return _GameEvents;
    }

    public static ListenerManager Get()
    {
        return _ListenerManager ?? new ListenerManager();
    }
}