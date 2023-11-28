using System.Reflection;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;
using SystemFile = System.IO;
using Il2cppFile = Il2CppSystem.IO.File;

namespace NextShip.Api.Utilities;

public sealed class AssetLoader
{
    private AssetBundle? Asset;
    public bool loaded;

    public AssetLoader(string fileName = "", string loaderName = "")
    {
        FileName = fileName;
        LoaderName = loaderName;
        if (fileName != "" || loaderName != "") Add();
    }

    public string LoaderName { get; }
    public string FileName { get; private set; }

    public AssetLoader LoadFile(string fileName)
    {
        FileName = fileName;
        return this;
    }

    public T Load<T>(string name) where T : Il2CppObjectBase
    {
        return Asset.LoadAsset<T>(name);
    }

    public List<T> LoadAll<T>() where T : Il2CppObjectBase
    {
        return Asset.LoadAllAsset<T>().ToList();
    }

    /*public AssetLoader LoadFromDisk()
    {
        if (FileName is "" or null) return this;

        var directory = FilesManager.GetDataDirectory("/Assets");
        var bytes = Il2cppFile.ReadAllBytes(SystemFile.Path.Combine(directory.FullName, "Assets", FileName));
        Asset = AssetBundle.LoadFromMemory(bytes);
        loaded = true;
        AssetManager.Get().Add(Asset);
        return this;
    }*/

    public AssetLoader LoadFromResources(Assembly assembly)
    {
        if (FileName is "" or null) return this;

        var name = assembly.GetManifestResourceNames().FirstOrDefault(n => n.Contains($"Assets.{FileName}"));
        if (name == null) return this;
        var Stream = assembly.GetManifestResourceStream(name);
        Asset = AssetBundle.LoadFromMemory(Stream.ReadFully());
        loaded = true;
        AssetManager.Get().Add(Asset);
        return this;
    }

    public void Add()
    {
        AssetManager.Get().Add(this);
    }

    public void Remove()
    {
        AssetManager.Get().Remove(this);
    }
}

public class AssetManager
{
    public static AssetManager? Instance;
    private readonly List<AssetBundle?> _assetBundles = new();

    private readonly List<AssetLoader?> _assetLoaders = new();

    public static AssetManager Get()
    {
        return Instance ??= new AssetManager();
    }

    public IReadOnlyList<AssetLoader?> GetLoaders()
    {
        return _assetLoaders;
    }

    public IReadOnlyList<AssetBundle?> GetAssetS()
    {
        return _assetBundles;
    }

    public void Add(AssetLoader? assetLoader)
    {
        _assetLoaders.Add(assetLoader);
    }

    public void Add(AssetBundle? assetBundle)
    {
        _assetBundles.Add(assetBundle);
    }

    public void Remove(AssetLoader? assetLoader)
    {
        _assetLoaders.Remove(assetLoader);
    }

    public AssetLoader? GetLoader(string name)
    {
        return _assetLoaders.Find(n => name == n.FileName || name == n.LoaderName);
    }
}