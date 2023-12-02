using System;
using Microsoft.Extensions.DependencyInjection;
using NextShip.Api.Interfaces;

namespace NextShip;

public class NextService : INextService
{
    private ServiceCollection _collection = new();
    public IServiceProvider _Provider { get; private set; }

    private bool BuildCompleted { get; set; }

    public void Build()
    {
        if (BuildCompleted) return;

        if (_collection.Count != 0)
        {
            _Provider = _collection.BuildServiceProvider();
            return;
        }

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
        _collection.Clear();
        _Provider = null;
        BuildCompleted = false;
    }

    public NextService Set(ServiceCollection collection)
    {
        _collection = collection;
        return this;
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
        var _Service = new NextService().Set(collection);
        _Service.Build();
        return _Service;
    }
}