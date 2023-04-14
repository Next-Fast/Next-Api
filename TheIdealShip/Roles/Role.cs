global using TheIdealShip.Modules;
using HarmonyLib;
using System;


namespace TheIdealShip.Roles
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

    public class RoleBase
    {
        public string RoleName { get; }
        public RoleId roleid { get; }
        public bool CanKill { get; }
        public bool CanVent { get; }
        public bool HasTask { get;}

        public RoleBase(string role, RoleId roleId)
        {
            RoleName = role;
            roleid = roleId;
            CanKill = false;
            CanVent = false;
            HasTask = true;
        }
    }
}