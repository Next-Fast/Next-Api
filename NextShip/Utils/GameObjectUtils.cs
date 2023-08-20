using HarmonyLib;
using UnityEngine;

namespace NextShip;

public static class GameObjectUtils
{
    public static int GameObjetCount = 0;
    // form TOHE
    /// <summary>
    ///     删除翻译文本（<see cref="TextTranslatorTMP" />）组件
    /// </summary>
    public static void DestroyTranslator(this GameObject obj)
    {
        TextTranslatorTMP[] translatorTMPs = obj.GetComponentsInChildren<TextTranslatorTMP>(true);
        if (translatorTMPs != null) translatorTMPs.Do(n => Object.Destroy(n));
    }

    /// <summary>
    ///     删除翻译文本（<see cref="TextTranslatorTMP" />）组件
    /// </summary>
    public static void DestroyTranslator(this MonoBehaviour obj)
    {
        obj.gameObject.DestroyTranslator();
    }

    /// <summary>
    ///     删除纠正位置（<see cref="AspectPosition" />）组件
    /// </summary>
    public static void DestroyAspectPosition(this GameObject gameObject)
    {
        AspectPosition[] aspectPositions = gameObject.GetComponentsInChildren<AspectPosition>(true);
        if (aspectPositions != null) aspectPositions.Do(n => Object.Destroy(n));
    }

    public static void DestroyComponents<T>(this GameObject gameObject) where T : Object
    {
        T[] components = gameObject.GetComponentsInChildren<T>(true);
        if (components != null) components.Do(n => Object.Destroy(n));
    }

    public static (GameObject, T) CreateGameObjet<T>(string name = null, Transform  parent = null) where T : Component
    {
        var gameObject = CreateGameObject(name, parent);
        return (gameObject, gameObject.AddComponent<T>());
    }

    public static GameObject CreateGameObject(string name = null, Transform parent = null)
    {
        GameObject gameObject = new GameObject();
        gameObject.name = name ?? $"NextShip_GameObject_{GameObjetCount}";
        if (parent != null) gameObject.transform.SetParent(parent);
        return gameObject;
    }
}