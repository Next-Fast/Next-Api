#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace NextShip.Manager;

public class InstanceManager
{
    public Dictionary<Type, object?> Instances = new();

    public void RegisterInstance<T>(T instance)
    {
        if (instance != null) 
            RegisterInstance(typeof(T), instance);
        else
        {
            Error("实例添加失败为空");
        }
    }

    public void RegisterInstance(Type type, object obj) => Instances[type] = obj;

    public T? Get<T>() where T : class => Get(typeof(T)) as T;

    public object? Get(Type type)
    {
        return Instances.FirstOrDefault(n => n.Key == type).Value;
    }
}