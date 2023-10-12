namespace NextShip.Updates;

public class DownLoadManager
{
    public static DownLoadManager Instance { get; private set; }
    public static DownLoadManager Get() => Instance ??= new DownLoadManager();

    public void Download(string url, string fileName, string filePath, DownLoadMode mode = DownLoadMode.Http)
    {
    }
}

public enum DownLoadMode
{
    Http,
    Downloader
}