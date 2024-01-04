namespace NextShip.Api.Services;

public class DependentService(DownloadService service)
{
    public string RootPath;
    
    public void SetPath(DirectoryInfo directoryInfo)
    {
        RootPath = directoryInfo.FullName;
    }

    public void Init()
    {
        
    }
}