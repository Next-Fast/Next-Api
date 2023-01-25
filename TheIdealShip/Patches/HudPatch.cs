using System.Linq;
using HarmonyLib;
using TheIdealShip.Utilities;
using TheIdealShip.Roles;
using static TheIdealShip.Languages.Language;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(TaskPanelBehaviour),nameof(TaskPanelBehaviour.SetTaskText))]
    class TaskPanelBehaviourPatch
    {
        public static void Postfix(TaskPanelBehaviour __instance)
        {
            var LPlayer = CachedPlayer.LocalPlayer.PlayerControl;
            var roleInfo = RoleInfo.GetRoleInfo(LPlayer, false);
            var modifierInfo = RoleInfo.GetRoleInfo(LPlayer, true);
            string roleText = "";
            string modifierText = "";
            if (roleInfo != null) roleText = Helpers.cs(roleInfo.color, GetString("Roles") + ":" + RoleInfo.GetRolesString(LPlayer, false) + "\n"+ roleInfo.TaskText +"\n");
            if (modifierInfo != null) modifierText = Helpers.cs(modifierInfo.color, GetString("Modifiers") + ":" + RoleInfo.GetRolesString(LPlayer, false, true) + "\n" + modifierInfo.TaskText + "\n");
            __instance.taskText.text = roleText + modifierText + "\n" + __instance.taskText.text;
//            __instance.taskText.text.Select(x => roleText + modifierText + ((roleInfo == null)&&(modifierInfo == null) ? "" : "\n") + x);
        }
    }
}