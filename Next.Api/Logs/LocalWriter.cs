using BepInEx;
using BepInEx.Core;

namespace Next.Api.Logs;

public class LocalWriter
{
    public LocalWriter(NextLog log)
    {
        if (!Directory.Exists(LogDir))
            Directory.CreateDirectory(LogDir);

        LogFileWriter = new StreamWriter(File.Open(Path.Combine(LogDir, log.LogSource.SourceName), FileMode.OpenOrCreate, FileAccess.Write))
        {
            AutoFlush = true,
        };

        LogFileWriter.WriteLine("NextLog Listener Start");
        LogFileWriter.WriteLine($"CurrentTime: {DateTime.Now:g}");
    }
    
    public static readonly string LogDir = Path.Combine(Paths.GameRootPath, "NextLogs");
    private readonly StreamWriter LogFileWriter;

    public LocalWriter Write(string str)
    {
        LogFileWriter.WriteLine(str);
        return this;
    }
}