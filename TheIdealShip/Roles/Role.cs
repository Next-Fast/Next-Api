using HarmonyLib;
using System;

namespace TheIdealShip.Roles
{
    [HarmonyPatch]
    public static class Role
    {
        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);
        public static void clearAndReloadRoles()
        {
            // Role 普通职业
            Sheriff.clearAndReload();

            // 中立
            Jester.clearAndReload();

            // Modifier 附加职业
            Flash.clearAndReload();
        }
    }

    enum RoleId
    {
        // Crewmate 船员
        Crewmate,
        Sheriff,

        // Impostor 内鬼
        Impostor,

        // Neutral 中立
        Jester,

        // Modifier 附加
        Flash

    }
}