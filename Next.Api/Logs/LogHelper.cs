using System.Reflection;
using BepInEx.Logging;


namespace Next.Api.Logs;

public static class LogHelper
{
    /*
    各消息作用:

    发生了致命错误，无法从中恢复 : A fatal error has occurred, which cannot be recovered from
    Fatal

    发生错误，但可以从中恢复 : An error has occured, but can be recovered from
    Error

    已发出警告，但并不一定意味着发生了错误 : A warning has been produced, but does not necessarily mean that something wrong has happened
    Warning

    应向用户显示的重要消息 : An important message that should be displayed to the user
    Message

    重要性较低的消息 :  A message of low importance
    Info

    可能只有开发人员感兴趣的消息 : A message that would likely only interest a developer
    Debug,
    */
    
    
    /// <summary>
    ///     一般信息
    /// </summary>
    /// <param name="Message"></param>
    public static void Info(string Message)
    {
        Log(Message, LogLevel.Info, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     报错
    /// </summary>
    /// <param name="Message"></param>
    public static void Error(string Message)
    {
        Log(Message, LogLevel.Error, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     测试
    /// </summary>
    /// <param name="Message"></param>
    public static void Debug(string Message)
    {
        Log(Message, LogLevel.Debug, Assembly.GetCallingAssembly());
    }

    public static void Fatal(string Message)
    {
        Log(Message, LogLevel.Fatal, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     警告
    /// </summary>
    /// <param name="Message"></param>
    public static void Warn(string Message)
    {
        Log(Message, LogLevel.Warning, Assembly.GetCallingAssembly());
    }


    public static void Message(string Message)
    {
        Log(Message, LogLevel.Message, Assembly.GetCallingAssembly());
    }

    public static void Exception(Exception exception)
    {
        Error(exception.ToString());
    }

    public static void Log(object @object, LogLevel errorLevel = LogLevel.None, Assembly? assembly = null)
    {
        var _assembly = assembly ?? Assembly.GetCallingAssembly();
        var log = NextLog.GetUseLog(_assembly);
        log.WriteToFile(@object, errorLevel);
    }

    public static void LogObject(object @object)
    {
        Log(@object, LogLevel.Error, Assembly.GetCallingAssembly());
    }
}