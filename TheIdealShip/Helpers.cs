using System.Text;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using TheIdealShip.Utilities;
using Il2CppInterop.Runtime.InteropTypes;
using System.Linq.Expressions;
using TheIdealShip.Roles;
using static TheIdealShip.Languages.Language;

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
                Warn("加载图片失败路径:" + path, filename : "Helpers");
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
                Warn("加载图片失败路径:" + path, filename: "Helpers");
            }
            return null;
        }

        public static Texture2D LoadTextureFromDisk(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                    var byteTexture = Il2CppSystem.IO.File.ReadAllBytes(path);
                    ImageConversion.LoadImage(texture, byteTexture, false);
                    return texture;
                }
            }
            catch
            {
                Warn("加载图片失败路径:" + path, filename: "Helpers");
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

        public static string stringOption(string str)
        {
            string s = "";
            if (str.Contains("</color>") && str.Contains("-"))
            {
                s = str.Substring(str.IndexOf("-") + 2);
                s = s.clearColor();
                s = str.Replace(s, GetString(s));
            }
            else
            if (str.Contains("</color>"))
            {
                s = str.clearColor();
                s = str.Replace(s, GetString(s));
            }
            else
            if (str.Contains("-"))
            {
                s = str.Substring(str.IndexOf("-") + 2);
                s = str.Replace(s, GetString(s));
            }
            else
            {
                s = GetString(str);
            }
            return s;
        }

        private static string clearColor(this string str)
        {
            string s = str.Replace("</color>", "");
            var found = s.IndexOf(">");
            s = s.Substring(found + 1);
            return s;
        }

        public static string ToColorString(this string text, Color color)
        {
            string colorString;
            colorString = "<color=" + ColorUtility.ToHtmlStringRGB(color) + ">" + text +"<color/>";
            return colorString;
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
       /*  public static void CWrite(string Text)
        {
            System.Console.WriteLine(Text);
         // TheIdealShipPlugin.Logger.LogInfo(Text);
        } */

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

        public static RoleInfo GetPlayerInfoForExile(this GameData.PlayerInfo exile)
        {
            var p = GetPlayerForId(exile.PlayerId);
            var info = RoleHelpers.GetRoleInfo(p);
            return info;
        }

        public static PlayerControl GetPlayerForExile(this GameData.PlayerInfo exile)
        {
            var p = GetPlayerForId(exile.PlayerId);
            return p;
        }

        public static void setDefaultLook(this PlayerControl target) {
            target.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void setLook(this PlayerControl target, string playerName, int colorId, string hatId, string visorId, string skinId, string petId )
        {
            target.RawSetColor(colorId);
            target.RawSetHat(hatId, colorId);
            target.RawSetVisor(visorId, colorId);
            target.RawSetName(playerName);

            SkinViewData nextSkin = FastDestroyableSingleton<HatManager>.Instance.GetSkinById(skinId).viewData.viewData;
            PlayerPhysics playerPhysics = target.MyPhysics;
            AnimationClip clip = null;
            var spriteAnim = playerPhysics.myPlayer.cosmetics.skin.animator;
            var currentPhysicsAnim = playerPhysics.Animations.Animator.GetCurrentAnimation();


            if (currentPhysicsAnim == playerPhysics.Animations.group.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;
            float progress = playerPhysics.Animations.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playerPhysics.myPlayer.cosmetics.skin.skin = nextSkin;
            playerPhysics.myPlayer.cosmetics.skin.UpdateMaterial();

            spriteAnim.Play(clip, 1f);
            spriteAnim.m_animator.Play("a", 0, progress % 1);
            spriteAnim.m_animator.Update(0f);

            if (target.cosmetics.currentPet) UnityEngine.Object.Destroy(target.cosmetics.currentPet.gameObject);
            target.cosmetics.currentPet = UnityEngine.Object.Instantiate<PetBehaviour>(FastDestroyableSingleton<HatManager>.Instance.GetPetById(petId).viewData.viewData);
            target.cosmetics.currentPet.transform.position = target.transform.position;
            target.cosmetics.currentPet.Source = target;
            target.cosmetics.currentPet.Visible = target.Visible;
            target.SetPlayerMaterialColors(target.cosmetics.currentPet.rend);
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
