global using TheIdealShip.Modules;
global using Role = TheIdealShip.Roles.Core.Role;
using HarmonyLib;
using System;


namespace TheIdealShip.Roles.Core
{
    [HarmonyPatch]
    public class Role
    {
        public static Random rnd = new Random((int)DateTime.Now.Ticks);
        public static void clearAndReloadRoles()
        {
            // Role 普通职业
            Sheriff.clearAndReload();
            Camouflager.clearAndReload();
            Illusory.clearAndReload();

            // 中立
            Jester.clearAndReload();
            SchrodingersCat.clearAndReload();

            // Modifier 附加职业
            Flash.clearAndReload();
        }
    }

    public enum RoleId
    {
        // Crewmate 船员
        Crewmate,
        Sheriff,
        Postman,

        // Impostor 内鬼
        Impostor,
        Camouflager,
        Illusory,

        // Neutral 中立
        Jester,
        SchrodingersCats,

        // Modifier 附加
        Flash,
        Lover

    }

    public enum RoleType
    {
        MainRole,
        ModifierRole,
        NotRole
    }
    public enum RoleTeam
    {
        Crewmate,
        Impostor,
        Neutral
    }
}