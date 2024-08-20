using System.Reflection;

namespace Next.Api.Logs;

public class LogConfigInfo(Assembly assembly, NextLog log)
{
    public readonly Assembly Assembly = assembly;
    public string LogName { get; set; } = string.Empty;
    public NextLog _Log { get; set; } = log;
    public readonly List<Assembly> UseAssembly = [assembly];
}