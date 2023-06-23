using System.Collections.Generic;
using System;
using System.Reflection;
using HarmonyLib;
using TheIdealShip.Utilities.Attribute;

namespace TheIdealShip.Manager;

public static class RegisterManager
{
/*     public static List<Action<Type>> registerList = new(); */
    public static void Registration(Assembly dll)
    {
        log.Info("Register: Start Registration(开始注册)", filename:"RegisterManager");

        foreach (Type type in dll.GetTypes())
        {
            Il2CppRegisterAttribute.Registration(type);
/*             registerList.Do(n => n.Invoke(type)); */
        }
    }
}