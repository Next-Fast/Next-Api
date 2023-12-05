using System.Text;
using BepInEx;
using BepInEx.Logging;

namespace NextShip.Api.Logs;

public sealed class log
{
    public static bool CreateEd;

    public StreamWriter? DiskWriter;

    public TextWriter? ConsoleWriter;

    static log()
    {
        System.Console.OutputEncoding = Encoding.UTF8;
    }

    private log(ManualLogSource logSource)
    {
        LogSource = logSource;
        ConsoleWriter = ConsoleManager.ConsoleStream;
        Instance = this;
    }


    public void CreateDiskLog(string name, string? path = null)
    {
        var stream = GetDiskLogStream(name, path);
        DiskWriter = new StreamWriter(stream);
        DiskWriter.AutoFlush = true;

        DiskWriter.WriteLine($"Next Ship Api Disk Log OutTime:{DateTime.Now:g}");
    }

    public Stream GetDiskLogStream(string name, string? path = null, bool outTime = false)
    {
        if (outTime)
        {
            var timeString = DateTime.Now.ToString("g")
                .Replace("/", "-").Replace(" ", "-").Trim();
            name = $"{name}_{timeString}.log";
        }

        path ??= string.Empty;
        
        var FilePath = path + name;
        
        if (!File.Exists(FilePath))
            return File.Create(FilePath);

        Stream stream;
        var count = 0;
        
        while (true)
        {
            count++;
            FilePath = path + name + $"_{count}";
            
            if (!File.Exists(FilePath))
            {
                stream = File.Create(FilePath);
                break;
            }
            
            stream = File.Open(FilePath, FileMode.Open);
            if (stream.Length == 0)
                break;
        }
        
        return stream;
    }

    public ManualLogSource LogSource { get; private set; }

    public static log? Instance { get; set; }

    public static log? Get(ManualLogSource logSource)
    {
        if (CreateEd)
        {
            Instance?.Set(logSource);
            return Instance;
        }

        var _log = new log(logSource);
        CreateEd = true;
        return _log;
    }

    private void Set(ManualLogSource logSource)
    {
        if (logSource != LogSource)
            LogSource = logSource;
    }

    internal void SendToFile(string? tag, string? filename, string text, LogLevel level = LogLevel.Info)
    {
        var logger = Instance?.LogSource;
        
        var t = DateTime.Now.ToString("HH:mm:ss");
        
        var log_text = $"[{t}]";
        
        if (tag != null) 
            log_text += $"[{tag}]";
        
        if (filename != null) 
            log_text += $"[{filename}]";
        
        log_text += text;
        
        switch (level)
        {
            case LogLevel.Info:
                logger?.LogInfo(log_text);
                break;
            case LogLevel.Warning:
                logger?.LogWarning(log_text);
                break;
            case LogLevel.Error:
                logger?.LogError(log_text);
                break;
            case LogLevel.Fatal:
                logger?.LogFatal(log_text);
                break;
            case LogLevel.Message:
                logger?.LogMessage(log_text);
                break;
            case LogLevel.Debug:
                logger?.LogDebug(log_text);
                break;
            case LogLevel.None:
                logger?.LogInfo(log_text);
                break;
            case LogLevel.All:
                break;
            default:
                logger?.LogWarning("Error:Invalid LogLevel");
                logger?.LogInfo(log_text);
                break;
        }
    }
}