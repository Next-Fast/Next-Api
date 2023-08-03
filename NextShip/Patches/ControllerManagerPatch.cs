using HarmonyLib;
using UnityEngine;

namespace NextShip.Patches;

[HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
internal class ControllerManagerUpdatePatch
{
    public static void Postfix()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.forcedEnd, false);
        if (Input.GetKeyDown(KeyCode.F2) && noGameEnd.getBool())
        {
            var Dia = RoleMenu.Dialogue;
            if (RoleMenu.isCreate) RoleMenu.CreateRoleMenu();
            if (Dia.gameObject.active == false)
                Dia.gameObject.SetActive(true);
            else
                Dia.gameObject.SetActive(false);
        }
    }
}