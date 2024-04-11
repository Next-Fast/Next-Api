using UnityEngine;

namespace NextShip.Api.Bases;

public class NKeyBind
{
    /// <summary>
    ///     0 last
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    ///     1 key or 2 keys or 3
    /// </summary>
    public int Mode { get; set; }

    public KeyCode[] keys { get; set; }

    public int KeyCount { get; set; }

    public Action _Action { get; set; }
}