using System.Reflection;
using TheIdealShip.Utilities.Attributes;

namespace TheIdealShip.Manager;

public static class RegisterManager
{
/*     public static List<Action<Type>> registerList = new(); */
    public static void Registration(Assembly dll)
    {
        Info("Register: Start Registration(开始注册)", filename: "RegisterManager");

        foreach (var type in dll.GetTypes()) Il2CppRegisterAttribute.Registration(type);
    }
}