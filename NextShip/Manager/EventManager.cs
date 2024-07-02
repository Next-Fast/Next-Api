using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.Api.Bases;
using NextShip.Api.Enums;
using NextShip.Api.Interfaces;
using NextShip.Api.RPCs;

namespace NextShip.Manager;

public class EventManager : IEventManager
{
    private readonly HashSet<IEventListener> EventListeners = [];

    private readonly List<INextEvent> RegisterEvents = [];

    private FastListener _listener;

    public EventManager()
    {
        _eventManager = this;
    }

    public static EventManager _eventManager { get; private set; }

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

    public FastListener GetFastListener()
    {
        return _listener ??= new FastListener();
    }

    public void CallEvent<T>(T @event) where T : INextEvent
    {
        foreach (var _event in RegisterEvents.FindAll(n => n.EventName == @event.EventName && n is T))
            @event.Call(_event);
    }

    public void RegisterListener(IEventListener listener)
    {
        EventListeners.Add(listener);
    }

    public void UnRegisterListener(IEventListener listener)
    {
        EventListeners.Remove(listener);
    }

    public void CallEvent(INextEvent @event)
    {
        foreach (var _event in RegisterEvents.FindAll(n => n.EventName == @event.EventName))
            @event.Call(_event);
    }

    public void CallListener(string name)
    {
        EventListeners.Do(n => n.On(name));
    }

    public void CallListenerToRPC(string name, object[] instances = null)
    {
        var writer = FastRpcWriter.StartNewRpcWriter(SystemRPCFlag.EventMessage);
        writer.Write(name);
        if (instances != null)
        {
            writer.WritePacked(instances.Length);
            foreach (var obj in instances) writer.Write(obj.GetType().Name);
        }

        writer.RPCSend();
    }

    public void CallListener(string name, object[] Instances)
    {
        EventListeners.Do(n => n.On(name, Instances));
    }

    public void CallListener(INextEvent @event)
    {
        EventListeners.Do(n => n.On(@event));
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