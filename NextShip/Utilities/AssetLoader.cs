using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Il2CppInterop.Runtime.InteropTypes;
using SystemFile = System.IO;
using Il2cppFile = Il2CppSystem.IO.File;
using NextShip.Manager;
using UnityEngine;

namespace NextShip.Utilities;

public class AssetLoader
{
    public string LoaderName { get; private set; }
    public string FileName { get; private set; }
    private AssetBundle Asset;
    public bool loaded = false;

    public AssetLoader(string fileName = "", string loaderName = "")
    {
        FileName = fileName;
        LoaderName = loaderName;
        if (fileName != "" || loaderName != "") Add();
    }

    public AssetLoader LoadFile(string fileName)
    {
        FileName = fileName;
        return this;
    }

    public T Load<T>(string name) where T : Il2CppObjectBase => Asset.LoadAsset<T>(name);
    public List<T> LoadAll<T>() where T : Il2CppObjectBase => Asset.LoadAllAsset<T>().ToList();

    public AssetLoader LoadFromDisk()
    {
        if (FileName is "" or null) return this;
        
        var directory = FilesManager.GetDataDirectory("/Assets");
        var bytes = Il2cppFile.ReadAllBytes(SystemFile.Path.Combine(directory.FullName, "Assets", FileName));
        Asset = AssetBundle.LoadFromMemory(bytes);
        loaded = true;
        AssetManager.Get().Add(Asset);
        return this;
    }

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

    public void Add() => AssetManager.Get().Add(this);
    public void Remove() => AssetManager.Get().Remove(this);
}

public class AssetManager
{
    public static AssetManager Instance;

    public static AssetManager Get() => Instance ??= new AssetManager();

    private readonly List<AssetLoader> _assetLoaders = new();
    private readonly List<AssetBundle> _assetBundles = new();

    public IReadOnlyList<AssetLoader> GetLoaders() => _assetLoaders;
    public IReadOnlyList<AssetBundle> GetAssetS() => _assetBundles;
    
    public void Add(AssetLoader assetLoader) => _assetLoaders.Add(assetLoader);
    public void Add(AssetBundle assetBundle) => _assetBundles.Add(assetBundle);
    public void Remove(AssetLoader assetLoader) => _assetLoaders.Remove(assetLoader);

    public AssetLoader GetLoader(string name) => _assetLoaders.Find(n => name == n.FileName || name == n.LoaderName);
}