using System;
using System.Reflection;
using HarmonyLib;
using NextShip.Listeners;
using NextShip.Listeners.Attributes;
using NextShip.Utilities.Attributes;

namespace NextShip.Manager;

public static class RegisterManager
{
/*     public static List<Action<Type>> registerList = new(); */
    public static void Registration(Assembly dll)
    {
        Info("Register: Start Registration(开始注册)", filename: "RegisterManager");
        dll.GetTypes().Do(StartRegistrations);
    }

    private static void StartRegistrations(Type type)
    {
        Il2CppRegisterAttribute.Registration(type);
        LoadAttribute.Registration(type);
        OptionLoad.Registration(type);
        EventListener.Registration(type);
        TranslateTag.Registration(type);
    }
}