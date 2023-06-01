using System.Reflection;

namespace TheIdealShip.Utils;

public class MethodUtils
{
    public static string GetNamespace() => MethodBase.GetCurrentMethod().DeclaringType.Namespace;
    public static string GetVoidName() => MethodBase.GetCurrentMethod().Name;
    public static string GetClassName() => System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
}