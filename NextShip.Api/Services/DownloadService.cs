using Microsoft.Extensions.Logging;

namespace NextShip.Api.Services;

public abstract class DownloadService(ILogger<DownloadService> logger,HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<DownloadService> _logger = logger;

    public void Download(Uri uri)
    {
        
    }
}