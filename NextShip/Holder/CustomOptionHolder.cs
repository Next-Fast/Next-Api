global using Types = NextShip.Modules.CustomOption.CustomOptionType;
global using static NextShip.CustomOptionHolder;
using NextShip.Roles;
using UnityEngine;

namespace NextShip;

public class CustomOptionHolder
{
    public static string[] rates = { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };
    public static string[] presets = { "Preset1", "Preset2", "Preset3", "Preset4", "Preset5" };
    public static Color GeneralColor = new(204f / 255f, 204f / 255f, 0, 1f);

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

    // public static string[] modeset = new string[]{"Classic","FreePlay"};
    public static string cs(Color c, string s)
    {
        var Cs = TextUtils.cs(c, s);
        return Cs;
    }

    public static void Load()
    {
        presetSelection = CustomOption.Create(0, Types.General, cs(GeneralColor, "Preset"), presets, null, true);
        //modeOption = CustomOption.Create(1,Types.General,"GameMode",modeset,null,true);
        noGameEnd = CustomOption.Create(2, Types.General, cs(GeneralColor, "NoGameEnd"), false, null, true);
        dummynumber = CustomOption.Create(16, Types.General, "DummyNumber", 0f, 0f, 15f, 1f);
        showExilePlayerRole =
            CustomOption.Create(3, Types.General, cs(GeneralColor, "ShowExilePlayerRole"), false, null, true);
        showExilePlayerConcreteRoleTeam = CustomOption.Create(17, Types.General,
            cs(GeneralColor, "ShowExilePlayerConcreteRoleTeam"), false, showExilePlayerRole);
        activateRoles =
            CustomOption.Create(4, Types.General, cs(GeneralColor, "Block Vanilla Roles"), true, null, true);

        crewmateRolesCountMin =
            CustomOption.Create(5, Types.General, cs(GeneralColor, "Minimum Crewmate Roles"), 15f, 0f, 15f, 1f);
        crewmateRolesCountMax =
            CustomOption.Create(6, Types.General, cs(GeneralColor, "Maximum Crewmate Roles"), 15f, 0f, 15f, 1f);
        neutralRolesCountMin =
            CustomOption.Create(7, Types.General, cs(GeneralColor, "Minimum Neutral Roles"), 15f, 0f, 15f, 1f);
        neutralRolesCountMax =
            CustomOption.Create(8, Types.General, cs(GeneralColor, "Maximum Neutral Roles"), 15f, 0f, 15f, 1f);
        impostorRolesCountMin =
            CustomOption.Create(9, Types.General, cs(GeneralColor, "Minimum Impostor Roles"), 15f, 0f, 15f, 1f);
        impostorRolesCountMax = CustomOption.Create(10, Types.General, cs(GeneralColor, "Maximum Impostor Roles"), 15f,
            0f, 15f, 1f);
        modifierRolesCountMin = CustomOption.Create(11, Types.General, cs(GeneralColor, "Minimum Modifier Roles"), 15f,
            0f, 15f, 1f);
        modifierRolesCountMax = CustomOption.Create(12, Types.General, cs(GeneralColor, "Maximum Modifier Roles"), 15f,
            0f, 15f, 1f);

        PlayerOption = CustomOption.Create(14, Types.General, "PlayerOption", false, null, true);
        disableHauntMenu = CustomOption.Create(13, Types.General, "disableHauntMenu", false, PlayerOption);
        PlayerGhostSpeed = CustomOption.Create(15, Types.General, "PlayerGhostSpeed", 3f, 1f, 10f, 0.5f, PlayerOption);
/*             disableServerKickPlayer = CustomOption.Create(18, Types.General, "DisableServerKickPlayer", false, null, true); */

        Jester.OptionLoad();
        Illusory.OptionLoad();
        Flash.OptionLoad();
        Lover.OptionLoad();
        Sheriff.OptionLoad();
        SchrodingersCat.OptionLoad();
        Camouflager.OptionLoad();
    }
}