using System;
using System.Collections.Generic;
using System.Linq;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class EventManager : IEventManager
{
    private readonly List<INextEvent> RegisterEvents = new();

    public void RegisterEvent(INextEvent @event)
    {
        @event.OnRegister(this);
        RegisterEvents.Add(@event);
    }

    public void UnRegisterEvent(INextEvent @event)
    {
        @event.OnUnRegister(this);
        RegisterEvents.Remove(@event);
    }

    public bool TryGetEvent(string eventName, out INextEvent @event)
    {
        if (RegisterEvents.Any(n => n.EventName == eventName))
        {
            @event = RegisterEvents.FirstOrDefault(n => n.EventName == eventName);
            return true;
        }

        @event = null;
        return false;
    }

    public T GetEvent<T>() where T : INextEvent
    {
        return (T)RegisterEvents.FirstOrDefault(n => n is T);
    }

    public INextEvent GetEvent(Type type)
    {
        return RegisterEvents.FirstOrDefault(n => n.GetType() == type);
    }
}