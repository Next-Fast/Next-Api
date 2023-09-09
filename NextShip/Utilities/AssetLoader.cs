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
    private readonly string FileName;
    private AssetBundle Asset;
    public bool loaded = false;

    public AssetLoader(string fileName)
    {
        FileName = fileName;
    }

    public T Load<T>(string name) where T : Il2CppObjectBase => Asset.LoadAsset<T>(name);
    public List<T> LoadAll<T>() where T : Il2CppObjectBase => Asset.LoadAllAsset<T>().ToList();

    public AssetLoader LoadFromDisk()
    {
        var directory = FilesManager.GetDataDirectory("/Assets");
        var bytes = Il2cppFile.ReadAllBytes(SystemFile.Path.Combine(directory.FullName, "Assets", FileName));
        Asset = AssetBundle.LoadFromMemory(bytes);
        loaded = true;
        return this;
    }

    public AssetLoader LoadFromResources(Assembly assembly)
    {
        var name = assembly.GetManifestResourceNames().FirstOrDefault(n => n.Contains($"Assets.{FileName}"));
        if (name == null) return this;
        var Stream = assembly.GetManifestResourceStream(name);
        Asset = AssetBundle.LoadFromMemory(Stream.ReadFully());
        loaded = true;
        return this;
    }
}