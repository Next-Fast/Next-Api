using System.Linq;
using HarmonyLib;
using UnityEngine;
using TheIdealShip.Modules;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    class ControllerManagerUpdatePatch
    {
            static bool GetKeysDown(params KeyCode[] keys)
            {
                if (keys.Any(k => Input.GetKeyDown(k)) && keys.All(k => Input.GetKey(k)))
                {
                    Helpers.CWrite($"KeyDown:{keys.Where(k => Input.GetKeyDown(k)).First()} in [{string.Join(",", keys)}]");
                    return true;
                }
                return false;
            }
        public static void Postfix()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                GameManager.Instance.RpcEndGame(GameOverReason.HumansByVote, false);
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
}