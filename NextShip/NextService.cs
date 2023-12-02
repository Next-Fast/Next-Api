using System;
using Microsoft.Extensions.DependencyInjection;
using NextShip.Api.Interfaces;

namespace NextShip;

public class NextService : INextService
{
    public IServiceProvider _Provider { get; private set; }

    private ServiceCollection _collection = new ();
    
    private bool BuildCompleted { get; set; }

    public NextService Set(ServiceCollection collection)
    {
        _collection = collection;
        return this;
    }

    public T Get<T>() => _Provider.GetService<T>();

    public object Get(Type type) => _Provider.GetService(type); 
        
    public void Build()
    {
        if(BuildCompleted) return;

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

    public static NextService Build(ServiceCollection collection)
    {
        var _Service = new NextService().Set(collection);
        _Service.Build();
        return _Service;
    }
}