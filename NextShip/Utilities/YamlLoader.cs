using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using YamlDotNet;
using YamlDotNet.RepresentationModel;

namespace NextShip.Utilities;

public class YamlLoader
{
    private readonly string FileName;
    public bool loaded = false;
    private YamlStream YamlStream;
    private YamlMappingNode mapping;
    
    public YamlLoader(string fileName)
    {
        FileName = fileName;
    }
    
    /*// form https://github.com/CognifyDev/ClashOfGods/blob/main/COG/Utils/Yaml.cs
    public void Load()
    {
        if (loaded) return;
        
        var rootNode = YamlStream.Documents[0].RootNode;

        if (rootNode is YamlMappingNode yamlMappingNode)
        {
            foreach (var nodeChild in  yamlMappingNode.Children)
            {
            }
        }
        
    }*/

    public YamlLoader LoadFromDisk(string path = null)
    {
        var FilePath = path ?? Path.Combine(Languages.LanguagePack.PPath, $"{FileName}.yaml");
        
        using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
        {
            YamlStream = new YamlStream();
            var textReader = new StreamReader(fs);
            
            YamlStream.Load(textReader);
        }
        
        loaded = true;
        return this;
    }

    public YamlLoader LoadFromResource(Assembly assembly)
    {
        var yaml = assembly.GetManifestResourceNames().FirstOrDefault(n => n == FileName + ".yaml");
        if (string.IsNullOrEmpty(yaml)) return this;
        var stream = assembly.GetManifestResourceStream(yaml);
        var textReader = new StreamReader(stream!);
        YamlStream.Load(textReader);
        
        loaded = true;
        return this;
    }
}

/*public class yamlNode
{
    private static List<yamlNode> AllYamlNodes;
    public static List<yamlNode> GetAllYamlNodes() => AllYamlNodes ??= new List<yamlNode>();

    public List<string> node;
    public string Key;
    public readonly object Value;

    public readonly List<yamlNode> ChildNode = new ();
    public yamlNode ParentNode;
    
    public yamlNode(string key, object value, yamlNode parent = null)
    {
        ParentNode = parent;
        parent?.ChildNode.Add(this);
        node = new List<string>();
        Key = key;
        if (parent != null)
        {
            node = parent.node;
        }
        
        node.Add(key);

        Value = value;
        GetAllYamlNodes().Add(this);
    }

    public static void LoadMode()
    {
        
    }
    
    public bool TryGetValue<T>(out T value)
    {
        if (Value == null)
        {
            value = default(T);
            return false;
        }

        value = (T)Value;
        return true;
    }
}*/