using System.Reflection;

namespace NextShip.Api.Utils;

public class MethodUtils
{
    /// <summary>
    ///     获取运行方法命名空间
    /// </summary>
    public static string GetNamespace()
    {
        return MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace ?? string.Empty;
    }

    /// <summary>
    ///     获取运行方法
    /// </summary>
    public static string? GetVoidName()
    {
        return MethodBase.GetCurrentMethod()?.Name;
    }

    /// <summary>
    ///     获取运行方法类
    /// </summary>
    /// <returns></returns>
    public static string? GetClassName()
    {
        return MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
    }
}