using System;
using static TheIdealShip.Languages.Language;
using UnityEngine;
using TheIdealShip.Modules;
using TheIdealShip.Roles;
using Types = TheIdealShip.Modules.CustomOption.CustomOptionType;

namespace TheIdealShip
{
    public class CustomOptionHolder
    {
        public static string[] rates = new string[]{"0%","10%","20%","30%","40%","50%","60%","70%","80%","90%","100%"};
        public static string[] presets = new string[]{"Preset1","Preset2","Preset3" ,"Preset4" ,"Preset5" };
        public static string[] modeset = new string[]{"Classic","FreePlay"};
        public static string cs(Color c, string s)
        {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", Helpers.ToByte(c.r), Helpers.ToByte(c.g), Helpers.ToByte(c.b), Helpers.ToByte(c.a), s);
        }

        public static CustomOption presetSelection;
        public static CustomOption modeOption;
        public static CustomOption nogameend;

        public static CustomOption sheriffSpawnRate;
        public static CustomOption sheriffCooldown;

        public static void Load()
        {
            presetSelection = CustomOption.Create(0, Types.General, cs(new Color(204f / 255f,204f / 255f,0,1f), "Preset"), presets, null, true);
            //modeOption = CustomOption.Create(1,Types.General,"GameMode",modeset,null,true);
            nogameend = CustomOption.Create(2,Types.General,"NoGameEnd",false,null,true);
            //                                     Id Tap分类            选项名                              父项   为父项
            sheriffSpawnRate = CustomOption.Create(20, Types.Crewmate, cs( Sheriff.color, "Sheriff"), rates, null, true);
            //                                    ID  Tap分类          选项名             默认 最小 最大 间隔   父项
            sheriffCooldown = CustomOption.Create(21, Types.Crewmate, "SheriffCooldown", 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
        }
    }
}