#nullable enable
namespace NextShip.Api.Bases;

public struct FastEventArgs
{
    public string Name { get; set; }

    public string EventName { get; set; }

    public object[] Instances { get; set; }

    public FastListener _FastListener { get; set; }

    public T? Get<T>() where T : class
    {
        return TryGet<T>(out var instance) ? instance : null;
    }

    public bool TryGet<T>(out T? instance) where T : class
    {
        instance = null;
        if (!TryGet(typeof(T), out var inGet)) return false;
        instance = inGet as T;
        return true;
    }

    public bool TryGet(Type type, out object? instance)
    {
        instance = null;
        if (!_FastListener.EventInstanceTypes.TryGetValue(EventName, out List<Type>? value)) return false;
        instance = Instances[value.IndexOf(type)];
        return true;
    }
}