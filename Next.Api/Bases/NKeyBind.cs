using UnityEngine;

namespace Next.Api.Bases;

public class NKeyBind(int priority, int mode, KeyCode[] keys, int keyCount, Action action)
{
    /// <summary>
    ///     0 last
    /// </summary>
    public int Priority { get; set; } = priority;

    /// <summary>
    ///     1 key or 2 keys or 3
    /// </summary>
    public int Mode { get; set; } = mode;

    public KeyCode[] keys { get; set; } = keys;

    public int KeyCount { get; set; } = keyCount;

    public Action _Action { get; set; } = action;
}