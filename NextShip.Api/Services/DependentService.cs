using System.Reflection;

namespace NextShip.Api.Services;

public class DependentService(HttpClient _client)
{
    public readonly HashSet<(Assembly, FileInfo)> Dlls = [];

    public readonly Queue<(Uri, string)> DownloadDependents = new();

    public readonly Queue<(Stream, string)> GenerateDependents = new();

    private DirectoryInfo Directory;
    private string RootPath;

    public void SetPath(DirectoryInfo directoryInfo)
    {
        RootPath = directoryInfo.FullName;
        Directory = directoryInfo;
    }

    public void BuildDependent()
    {
        while (DownloadDependents.Count > 0)
        {
            var (url, name) = DownloadDependents.Dequeue();
            var stream = _client.GetStreamAsync(url).Result;
            GenerateDependents.Enqueue((stream, name));
        }

        while (GenerateDependents.Count > 0)
        {
            var (stream, name) = GenerateDependents.Dequeue();
            var generateFile = File.Create(Directory.FullName + "/" + name);
            stream.CopyToAsync(generateFile);
        }
    }

    public void LoadDependent()
    {
        var files = Directory.GetFiles();
        foreach (var file in files)
        {
            if (file.Extension == ".dll")
            {
                Dlls.Add((Assembly.LoadFile(file.FullName), file));
                continue;
            }

            file.Delete();
        }
    }
}