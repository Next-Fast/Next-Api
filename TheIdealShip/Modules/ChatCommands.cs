using HarmonyLib;
using TheIdealShip.Utilities;
using TheIdealShip.Roles;
using TheIdealShip.RPC;

namespace TheIdealShip.Modules
{
    [HarmonyPatch]
    public static class ChatCommands
    {

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
        private static class SendChatPatch
        {
            public static bool Prefix(ChatController __instance)
            {
                __instance.TimeSinceLastMessage = 3f;
                var text = __instance.TextArea.text;
                string[] args = text.Split(' ');
                var canceled = false;
                var cancelVal = "";
                if (CustomOptionHolder.noGameEnd.getBool())
                {
                    switch (args[0])
                    {
                        case "/role":
                            foreach (var n in CachedPlayer.AllPlayers)
                            {
                                foreach (var rn in RoleInfo.allRoleInfos)
                                {
                                    if (n.Data.PlayerName == args[1] )
                                    {
                                        if (args[2] == rn.namekey || args[2] == rn.name)
                                        {
                                            RPCProcedure.setRole((byte)rn.roleId, n.PlayerId);
                                        }
                                    }
                                    if (args[1] == rn.namekey || args[1] == rn.name)
                                    {
                                        RPCProcedure.setRole((byte)rn.roleId, CachedPlayer.LocalPlayer.PlayerId);
                                    }
                                }
                            }
                            break;
                        case "/isD":
                            if (args[1] == "true")
                            {
                                CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead = true;
                            }
                            if (args[1] == "false")
                            {
                                CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead = false;
                            }
                            break;
                    }
                }
                if (canceled)
                {
                    __instance.TextArea.Clear();
                    __instance.TextArea.SetText(cancelVal);
                    __instance.quickChatMenu.ResetGlyphs();
                }
                return !canceled;
            }
        }
    }
}