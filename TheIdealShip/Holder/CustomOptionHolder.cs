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
       // public static string[] modeset = new string[]{"Classic","FreePlay"};
        public static string cs(Color c, string s)
        {
            var Cs = Helpers.cs(c,s);
            return Cs;
        }
        public static Color GeneralColor;

        public static CustomOption presetSelection;
        public static CustomOption modeOption;
        public static CustomOption noGameEnd;
        public static CustomOption activateRoles;
        public static CustomOption crewmateRolesCountMin;
        public static CustomOption crewmateRolesCountMax;
        public static CustomOption neutralRolesCountMin;
        public static CustomOption neutralRolesCountMax;
        public static CustomOption impostorRolesCountMin;
        public static CustomOption impostorRolesCountMax;
        public static CustomOption modifierRolesCountMin;
        public static CustomOption modifierRolesCountMax;

        public static CustomOption sheriffSpawnRate;
        public static CustomOption sheriffCooldown;
        public static CustomOption sheriffshootNumber;

        public static void Load()
        {
            GeneralColor = new Color(204f / 255f, 204f / 255f, 0, 1f);

            presetSelection = CustomOption.Create(0, Types.General, cs(GeneralColor, "Preset"), presets, null, true);
            //modeOption = CustomOption.Create(1,Types.General,"GameMode",modeset,null,true);
            noGameEnd = CustomOption.Create(2,Types.General,cs(GeneralColor,"NoGameEnd"),false,null,true);
            activateRoles = CustomOption.Create(3,Types.General,cs(GeneralColor,"Block Vanilla Roles"),true,null,true);

            crewmateRolesCountMin = CustomOption.Create(4,Types.General,cs(GeneralColor,"Minimum Crewmate Roles"),15f, 0f, 15f, 1f);
            crewmateRolesCountMax = CustomOption.Create(5,Types.General,cs(GeneralColor,"Maximum Crewmate Roles"),15f, 0f, 15f, 1f);
            neutralRolesCountMin = CustomOption.Create(6,Types.General,cs(GeneralColor,"Minimum Neutral Roles"),15f, 0f, 15f, 1f);
            neutralRolesCountMax = CustomOption.Create(7,Types.General,cs(GeneralColor,"Maximum Neutral Roles"),15f, 0f, 15f, 1f);
            impostorRolesCountMin = CustomOption.Create(8, Types.General, cs(GeneralColor, "Minimum Impostor Roles"), 15f, 0f, 15f, 1f);
            impostorRolesCountMax = CustomOption.Create(9, Types.General, cs(GeneralColor, "Maximum Impostor Roles"), 15f, 0f, 15f, 1f);
            modifierRolesCountMin = CustomOption.Create(10, Types.General, cs(GeneralColor, "Minimum Modifier Roles"), 15f, 0f, 15f, 1f);
            modifierRolesCountMax = CustomOption.Create(11, Types.General, cs(GeneralColor, "Maximum Modifier Roles"), 15f, 0f, 15f, 1f);

            //                                     Id Tap分类            选项名                              父项   为父项
            sheriffSpawnRate = CustomOption.Create(20, Types.Crewmate, cs( Sheriff.color, "Sheriff"), rates, null, true);
            //                                    ID  Tap分类          选项名             默认 最小 最大 间隔   父项
            sheriffCooldown = CustomOption.Create(21, Types.Crewmate, "SheriffCooldown", 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
            sheriffshootNumber = CustomOption.Create(22, Types.Crewmate, "ShootNumber", 5f,1f,15f,1f,sheriffSpawnRate);
        }
    }
}