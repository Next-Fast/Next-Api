using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Logging;

namespace Next.Api.Logs;

public class NextLog(Assembly _assembly, ManualLogSource logSource)
{
    public Assembly Assembly = _assembly;
    public ManualLogSource LogSource = logSource;
    public bool UseLocalWrite { get; set; } = false;
    private LocalWriter? _writer;
    public LocalWriter? Writer
    {
        get
        {
            if (UseLocalWrite)
            {
                return _writer ??= new LocalWriter(this);
            }

            return null;
        }
    }

    public static List<LogConfigInfo> LogConfigInfos = [];
    public static NextLog? MainLog { get; private set; }

    public static void RemoveLog(Assembly? assembly = null)
    {
        var _assembly = assembly ?? Assembly.GetCallingAssembly();
        foreach (var info in LogConfigInfos.Where(n => n.UseAssembly.Contains(_assembly)))
            info.UseAssembly.Remove(_assembly);
    }

    public static NextLog? SetLog(Assembly assembly)
    {
        if (LogConfigInfos.All(n => n.Assembly != assembly))
            return null;

        var info = LogConfigInfos.First(n => n.Assembly == assembly);
        info.UseAssembly.Add(assembly);
        
        return info._Log;
    }

    public static NextLog CreateMainLog(ManualLogSource logSource)
    {
        if (ConsoleManager.ConsoleEnabled) 
            System.Console.OutputEncoding = Encoding.UTF8;
        
        var assembly = Assembly.GetCallingAssembly();
        MainLog = new NextLog(assembly, logSource);
        RemoveLog(assembly);

        LogConfigInfos.Add(new LogConfigInfo(assembly, MainLog)
        {
            LogName = logSource.SourceName
        });
        return MainLog;
    }

    public static NextLog GetUseLog(Assembly? assembly = null)
    {
        var _assembly = assembly ?? Assembly.GetCallingAssembly();
        var log = LogConfigInfos.FirstOrDefault(n => n.UseAssembly.Contains(_assembly))?._Log;
        if (log != null)
            return log;

        if (MainLog == null) return CreateMainLog(_assembly.GetLogSource());
        
        LogConfigInfos.First(n => n._Log == MainLog).UseAssembly.Add(_assembly);
        return MainLog;
    }

    public NextLog WriteToFile(object @object, LogLevel errorLevel = LogLevel.None)
    {
        var Message = @object as string;
        switch (errorLevel)
        {
            case LogLevel.Message:
                LogSource.LogMessage(Message);
                break;
            case LogLevel.Error:
                LogSource.LogError(Message);
                break;
            case LogLevel.Warning:
                LogSource.LogWarning(Message);
                break;
            case LogLevel.Fatal:
                LogSource.LogFatal(Message);
                break;
            case LogLevel.Info:
                LogSource.LogInfo(Message);
                break;
            case LogLevel.Debug:
                LogSource.LogDebug(Message);
                break;
            case LogLevel.None:
            case LogLevel.All:
            default:
                LogSource.LogInfo(Message);
                goto Writer;
        }
        Writer:
        Writer?.Write($"[FastLog, Level: {errorLevel}] {Message}");
        return this;
    }
}