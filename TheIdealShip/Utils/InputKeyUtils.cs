using System.Linq;
using UnityEngine;

namespace TheIdealShip.Utils;

public static class InputKeyUtils
{
    public static bool GetKeysDown(params KeyCode[] keys)
    {
        if (keys.Any(k => Input.GetKeyDown(k)) && keys.All(k => Input.GetKey(k)))
        {
            Warn($"KeyDown:{keys.Where(k => Input.GetKeyDown(k)).First()} in [{string.Join(",", keys)}]");
            return true;
        }
        return false;
    }

    public static bool GetKeyDown(KeyCode key)
    {
        bool has = Input.GetKeyDown(key);
        Info($"GetKeyDown:{key} : {has} form {MethodUtils.GetClassName()}");
        return has;
    }
}