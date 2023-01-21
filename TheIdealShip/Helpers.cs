using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Hazel;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections;
using TheIdealShip.Utilities;
using Il2CppInterop.Runtime.InteropTypes;
using System.Linq.Expressions;
using Il2CppInterop.Runtime;

namespace TheIdealShip
{
    public static class Helpers
    {
        private static Sprite ModStamp;
        public static Dictionary<string, Sprite> CachedSprites = new();
        public static Sprite LoadSpriteFromResources (String path,float pixelsPerUnit)
        {
            try
            {
                if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
                Texture2D texture = LoadTextureFromResources(path);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
                return CachedSprites[path + pixelsPerUnit] = sprite;
            }
            catch
            {
                CWrite("Error loading sprite from path: " + path);
            }
            return null;
        }

        public static unsafe Texture2D LoadTextureFromResources(string path)
        {
            try
            {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var length = stream.Length;
                var byteTexture = new Il2CppStructArray<byte>(length);
                stream.Read(new Span<byte>(IntPtr.Add(byteTexture.Pointer, IntPtr.Size * 4).ToPointer(), (int)length));
                ImageConversion.LoadImage(texture, byteTexture, false);
                return texture;
            }
            catch
            {
                CWrite("Error loading texture from resources: " + path);
            }
            return null;
        }

        public static string cs(Color c,string s)
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>",ToByte(c.r),ToByte(c.g),ToByte(c.b),ToByte(c.a),s);
        }

        public static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static Sprite getModStamp()
        {
            if (ModStamp) return ModStamp;
            return ModStamp = Helpers.LoadSpriteFromResources("TheIdealShip.Resources.ModStamp.png", 150f);
        }
/*
        public static string GetDev()
        {
            string DevText = "\n";
            if(TheIdealShipPlugin.IsDev)
            {
                DevText = DevText+"Dev:"+TheIdealShipPlugin.BuildTime;
            }
            return DevText;
        }

        public static string GetNTime()
        {
            string time;
            time = DateTime.Now.ToShortDateString().ToString();
            return time;
        }
*/
        public static void CWrite(string Text)
        {
            System.Console.WriteLine(Text);
         // TheIdealShipPlugin.Logger.LogInfo(Text);
        }

        public static PlayerControl GetPlayerForId(byte id)
        {
           foreach (var AP in CachedPlayer.AllPlayers)
           {
            if (AP.PlayerId == id)
            {
                return AP;
            }
           }
            return null;
        }

        public static T CastFast<T>(this Il2CppObjectBase obj) where T : Il2CppObjectBase
        {
            if (obj is T casted) return casted;
            return obj.Pointer.CastFast<T>();
        }

        public static T CastFast<T>(this IntPtr ptr) where T : Il2CppObjectBase
        {
            return CastHelper<T>.Cast(ptr);
        }

        private static class CastHelper<T> where T : Il2CppObjectBase
        {
            public static Func<IntPtr, T> Cast;
            static CastHelper()
            {
                var constructor = typeof(T).GetConstructor(new[] { typeof(IntPtr) });
                var ptr = Expression.Parameter(typeof(IntPtr));
                var create = Expression.New(constructor!, ptr);
                var lambda = Expression.Lambda<Func<IntPtr, T>>(create, ptr);
                Cast = lambda.Compile();
            }
        }
    }
}
