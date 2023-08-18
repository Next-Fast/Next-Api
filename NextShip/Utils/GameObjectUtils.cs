using HarmonyLib;
using UnityEngine;

namespace NextShip;

public static class GameObjectUtils
{
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
}