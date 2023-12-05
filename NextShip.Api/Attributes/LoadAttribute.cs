using System.Collections;
using System.Reflection;

namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor)]
public sealed class LoadAttribute : Attribute
{
    public readonly LoadMode Mode;

    public LoadAttribute(LoadMode mode = LoadMode.Load)
    {
        Mode = mode;
    }

    public static void Registration(Type type)
    {
        Info("Start Registration", filename: MethodUtils.GetClassName());
        ConstructorInfo? constructor;
        if (
            type.GetCustomAttribute<LoadAttribute>() != null
            &&
            (constructor = type.GetConstructor
                (
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.NonPublic,
                    Array.Empty<Type>()
                )
            )
            != null)
            constructor.Invoke(null, null);

        foreach (var variableMethodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic |
                                                           BindingFlags.Public))
        {
            if (variableMethodInfo.Name is "CoLoad" or "Load")
            {
                if
                    (
                        (
                            type.IsDefined(typeof(LoadAttribute))
                            ||
                            variableMethodInfo.IsDefined(typeof(LoadAttribute))
                        )
                        &&
                        variableMethodInfo.ReturnType == typeof(IEnumerator))
                    /*LoadManager.AllLoad.Add(variableMethodInfo.Invoke(null, null) as IEnumerator);*/
                    continue;

                if (type.IsDefined(typeof(LoadAttribute)))
                {
                    variableMethodInfo.Invoke(null, null);
                    continue;
                }
            }

            if (variableMethodInfo.GetCustomAttribute<LoadAttribute>() != null) variableMethodInfo.Invoke(null, null);
        }

        Info($"Statically Initialized Class {type}", "LoadAttribute");
    }
}