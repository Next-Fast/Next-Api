using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace TheIdealShip.Roles
{
    enum RoleId
    {
        // Crewmate 船员
        Crewmate,
        Sheriff,

        // Impostor 内鬼
        Impostor

        // 中立

        // 附加
    }

    [HarmonyPatch]
    public static class Roles
    {
        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        public static void clearAndReloadRoles()
        {

        }
    }
}