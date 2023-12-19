using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NextShip.Api.Interfaces;

namespace NextShip.Services;

public class NextService : INextService
{
    public static readonly HashSet<NextService> Services = new();

    public NextService()
    {
        Services.Add(this);
    }

    public NextService(IServiceProvider provider) : this()
    {
        _Provider = provider;
        BuildCompleted = true;
    }

    public IServiceProvider _Provider { get; private set; }

    private bool BuildCompleted { get; set; }

    public void Build()
    {
        if (BuildCompleted) return;

        var collection = new ServiceCollection();

        _Provider = collection.BuildServiceProvider();
    }


    public void Rebuild()
    {
        Reset();
        Build();
    }

    public void Reset()
    {
        _Provider = null;
        BuildCompleted = false;
    }

    public static NextService Build(IServiceBuilder builder)
    {
        var service = builder.Build();
        service.Build();

        return (NextService)service;
    }

    public T Get<T>()
    {
        return _Provider.GetService<T>();
    }

    public object Get(Type type)
    {
        return _Provider.GetService(type);
    }

    public static NextService Build(ServiceCollection collection)
    {
        return (NextService)new ServiceBuilder().Set(collection).Build();
    }
}