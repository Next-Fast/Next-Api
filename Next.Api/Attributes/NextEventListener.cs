using System.Reflection;
using Next.Api.Interfaces;

namespace Next.Api.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class NextEventListener : Attribute
{
    public static void RegisterFormAssembly(Assembly assembly, IEventManager manager)
    {
    }

    public static void RegisterFormService(IServiceProvider provider, IEventManager manager)
    {
    }
}