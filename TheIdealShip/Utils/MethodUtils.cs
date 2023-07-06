using System.Reflection;

namespace TheIdealShip.Utils;

public class MethodUtils
{
    /// <summary>
    /// 获取运行方法命名空间
    /// </summary>
    public static string GetNamespace() => MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace;
    
    /// <summary>
    ///  获取运行方法
    /// </summary>
    public static string GetVoidName() => MethodBase.GetCurrentMethod()?.Name;
    
    /// <summary>
    /// 获取运行方法类
    /// </summary>
    /// <returns></returns>
    public static string GetClassName() => MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
}