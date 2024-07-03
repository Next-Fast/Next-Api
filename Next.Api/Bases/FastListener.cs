using HarmonyLib;

namespace Next.Api.Bases;

public class FastListener
{
    private readonly List<Action<FastEventArgs>> AllListener = [];

    public readonly Dictionary<string, List<Type>> EventInstanceTypes = new();

    public int Register(Action<FastEventArgs> action)
    {
        AllListener.Add(action);
        return AllListener.IndexOf(action);
    }

    public void UnRegister(int id)
    {
        AllListener.RemoveAt(id);
    }

    public void Call(string eventName, object[] instances = null, string name = null)
    {
        if (instances != null)
        {
            EventInstanceTypes[eventName] = [];
            foreach (var tp in instances) EventInstanceTypes[eventName].Add(tp.GetType());
        }

        AllListener.Do(n => n.Invoke(new FastEventArgs
        {
            Name = name,
            EventName = eventName,
            Instances = instances,
            _FastListener = this
        }));
    }
}