global using Types = TheIdealShip.Modules.CustomOption.CustomOptionType;
global using static TheIdealShip.CustomOptionHolder;
using TheIdealShip.Modules;
using TheIdealShip.Roles;
using UnityEngine;

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
        public static Color GeneralColor = new Color(204f / 255f, 204f / 255f, 0, 1f);

        public static CustomOption presetSelection;
        // public static CustomOption modeOption;
        public static CustomOption noGameEnd;
        public static CustomOption showExilePlayerRole;
        public static CustomOption showExilePlayerConcreteRoleTeam;
        public static CustomOption activateRoles;
        public static CustomOption crewmateRolesCountMin;
        public static CustomOption crewmateRolesCountMax;
        public static CustomOption neutralRolesCountMin;
        public static CustomOption neutralRolesCountMax;
        public static CustomOption impostorRolesCountMin;
        public static CustomOption impostorRolesCountMax;
        public static CustomOption modifierRolesCountMin;
        public static CustomOption modifierRolesCountMax;
        public static CustomOption disableHauntMenu;
        public static CustomOption PlayerOption;
        public static CustomOption PlayerGhostSpeed;
        public static CustomOption dummynumber;
        public static CustomOption disableServerKickPlayer;

        public static CustomOption illusorySpawnRate;
        public static CustomOption illusoryCooldown;
        public static CustomOption illusoryDuration;

        public static CustomOption sheriffSpawnRate;
        public static CustomOption sheriffCooldown;
        public static CustomOption sheriffshootNumber;

        public static CustomOption jesterSpawnRate;
        public static CustomOption jesterCanCallEmergency;

        public static CustomOption SchrodingersCatRate;

        public static CustomOption flashSpawnRate;
        public static CustomOption flashSpeed;

        public static CustomOption LoverSpawnRate;
        public static CustomOption LoverIsEvilProbability;
        public static CustomOption LoverDieForLove;
        public static CustomOption LoverPrivateChat;

        public static void Load()
        {

            presetSelection = CustomOption.Create(0, Types.General, cs(GeneralColor, "Preset"), presets, null, true);
            //modeOption = CustomOption.Create(1,Types.General,"GameMode",modeset,null,true);
            noGameEnd = CustomOption.Create(2, Types.General,cs(GeneralColor,"NoGameEnd"),false,null,true);
            dummynumber = CustomOption.Create(16, Types.General, "DummyNumber", 0f, 0f, 15f, 1f);
            showExilePlayerRole = CustomOption.Create(3, Types.General, cs(GeneralColor,"ShowExilePlayerRole"), false,null,true);
            showExilePlayerConcreteRoleTeam = CustomOption.Create(17, Types.General, cs(GeneralColor,"ShowExilePlayerConcreteRoleTeam"),false,showExilePlayerRole);
            activateRoles = CustomOption.Create(4,Types.General,cs(GeneralColor,"Block Vanilla Roles"),true,null,true);

            crewmateRolesCountMin = CustomOption.Create(5, Types.General,cs(GeneralColor,"Minimum Crewmate Roles"),15f, 0f, 15f, 1f);
            crewmateRolesCountMax = CustomOption.Create(6, Types.General,cs(GeneralColor,"Maximum Crewmate Roles"),15f, 0f, 15f, 1f);
            neutralRolesCountMin = CustomOption.Create(7, Types.General,cs(GeneralColor,"Minimum Neutral Roles"),15f, 0f, 15f, 1f);
            neutralRolesCountMax = CustomOption.Create(8, Types.General,cs(GeneralColor,"Maximum Neutral Roles"),15f, 0f, 15f, 1f);
            impostorRolesCountMin = CustomOption.Create(9, Types.General, cs(GeneralColor, "Minimum Impostor Roles"), 15f, 0f, 15f, 1f);
            impostorRolesCountMax = CustomOption.Create(10, Types.General, cs(GeneralColor, "Maximum Impostor Roles"), 15f, 0f, 15f, 1f);
            modifierRolesCountMin = CustomOption.Create(11, Types.General, cs(GeneralColor, "Minimum Modifier Roles"), 15f, 0f, 15f, 1f);
            modifierRolesCountMax = CustomOption.Create(12, Types.General, cs(GeneralColor, "Maximum Modifier Roles"), 15f, 0f, 15f, 1f);

            PlayerOption = CustomOption.Create(14, Types.General, "PlayerOption", false, null, true);
            disableHauntMenu = CustomOption.Create(13, Types.General, "disableHauntMenu", false, PlayerOption);
            PlayerGhostSpeed = CustomOption.Create(15, Types.General, "PlayerGhostSpeed", 3f, 1f, 10f, 0.5f, PlayerOption);
            disableServerKickPlayer = CustomOption.Create(18, Types.General, "DisableServerKickPlayer", false, null, true);

            illusorySpawnRate = CustomOption.Create(60, Types.Impostor, cs(Illusory.color, "Illusory"), rates, null, true);
            illusoryCooldown = CustomOption.Create(61, Types.Impostor, "Illusory Cooldown", 30f, 10f, 60f, 2.5f, illusorySpawnRate);
            illusoryDuration = CustomOption.Create(62, Types.Impostor, "Illusory Duration", 10f, 5f, 20f, 1f, illusorySpawnRate);

            //                                     Id Tap分类            选项名                              父项   为父项
            sheriffSpawnRate = CustomOption.Create(20, Types.Crewmate, cs(Sheriff.color, "Sheriff"), rates, null, true);
            //                                    ID  Tap分类          选项名             默认 最小 最大 间隔   父项
            sheriffCooldown = CustomOption.Create(21, Types.Crewmate, "SheriffCooldown", 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
            sheriffshootNumber = CustomOption.Create(22, Types.Crewmate, "ShootNumber", 5f,1f,15f,1f,sheriffSpawnRate);

            // 中立职业
            jesterSpawnRate = CustomOption.Create(150, Types.Neutral, cs(Jester.color, "Jester"), rates, null, true);
            jesterCanCallEmergency = CustomOption.Create(151, Types.Neutral, "CanCallEmergency", true, jesterSpawnRate);

            SchrodingersCatRate = CustomOption.Create(161, Types.Neutral, cs(SchrodingersCat.color, "Schrodinger's Cat"), rates, null, true);

            // modifier 附加职业
            flashSpawnRate = CustomOption.Create(100, Types.Modifier, cs(Flash.color, "Flash"), rates, null, true);
            flashSpeed = CustomOption.Create(101, Types.Modifier, "Speed", 5f, 1f, 10f, 0.5f, flashSpawnRate);

            LoverSpawnRate = CustomOption.Create(110, Types.Modifier, cs(Lover.Color, "Lover"), rates, null, true);
            LoverIsEvilProbability = CustomOption.Create(111, Types.Modifier, "Evil Lover Probability", rates, LoverSpawnRate);
            LoverDieForLove = CustomOption.Create(112, Types.Modifier, "Die For Love", true, LoverSpawnRate);
            LoverPrivateChat = CustomOption.Create(113, Types.Modifier, "Lover Private Chat", false, LoverSpawnRate);

        }
    }
}