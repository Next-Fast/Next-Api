using HarmonyLib;

namespace NextShip;

[HarmonyPatch(typeof(TaskPanelBehaviour), nameof(TaskPanelBehaviour.SetTaskText))]
internal class TaskPanelBehaviourPatch
{
/*     public static void Postfix(TaskPanelBehaviour __instance)
    {
        var LPlayer = CachedPlayer.LocalPlayer.PlayerControl;
        var roleInfo = RoleHelpers.GetRoleInfo(LPlayer, false);
        var modifierInfo = RoleHelpers.GetRoleInfo(LPlayer, true);
        string roleText = "";
        string modifierText = "";
        if (roleInfo != null) roleText = Helpers.cs(roleInfo.color, GetString("Roles") + ":" + RoleHelpers.GetRolesString(LPlayer, false) + "\n" + roleInfo.TaskText + "\n");
        if (modifierInfo != null) modifierText = Helpers.cs(modifierInfo.color, GetString("Modifiers") + ":" + RoleHelpers.GetRolesString(LPlayer, false, true) + "\n" + (modifierInfo.roleId == RoleId.Lover && RoleHelpers.getLover2() != null ? string.Format(modifierInfo.TaskText, RoleHelpers.getLover2().name) : modifierInfo.TaskText) + "\n");
        __instance.taskText.text = roleText + modifierText + "\n" + __instance.taskText.text;
    } */
}