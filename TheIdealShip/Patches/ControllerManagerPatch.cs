using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TheIdealShip.Patches;
[HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
class ControllerManagerUpdatePatch
{
    public static void Postfix()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.forcedEnd, false);
        }
        if (Input.GetKeyDown(KeyCode.F2) && CustomOptionHolder.noGameEnd.getBool())
        {
            var Dia = RoleMenu.Dialogue;
            if (RoleMenu.isCreate)
            {
                RoleMenu.CreateRoleMenu();
            }
            if (Dia.gameObject.active == false)
            {
                Dia.gameObject.SetActive(true);
            }
            else
            {
                Dia.gameObject.SetActive(false);
            }
        }
    }
}