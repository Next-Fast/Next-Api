namespace NextShip.Manager;

public class Manager<T> where T : new()
{
    protected static T _instance;
    
    public static T Instance => Get();

    public static T Get() => _instance ??= new T();
}