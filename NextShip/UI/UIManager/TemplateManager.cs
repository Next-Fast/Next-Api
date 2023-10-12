using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NextShip.UI.Interface;
using UnityEngine;

namespace NextShip.UI.UIManager;

public class TemplateManager
{
    private static TemplateManager _instance;
    public static TemplateManager Instance
    {
        get
        {
            return _instance ??= new TemplateManager();
        }

        protected set => _instance = value;
    }

    public List<GameObject> AllTemplateGameObjects = new();
    public HashSet<IUITemplate> AllTemplates = new();

    public GameObject Get<T>() where T : IUITemplate, new()
    {
        var uiTemplates = AllTemplates.FirstOrDefault(n => n is T);
        if (uiTemplates != null) return uiTemplates.GameObject;
        var uiTemplate = new T();
        return uiTemplate.Generate().GameObject;
    }

    public bool TryGet(string name, [AllowNull] out GameObject gameObject)
    {
        gameObject = AllTemplateGameObjects.FirstOrDefault(n => n.name == name);
        var te = AllTemplates.FirstOrDefault(n => n.Name == name);
        if (gameObject == null) gameObject = te?.GameObject;
        if (gameObject == null) gameObject = te?.Generate().GameObject;
        return gameObject;
    }
}