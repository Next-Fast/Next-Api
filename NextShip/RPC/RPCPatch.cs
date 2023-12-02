using HarmonyLib;
using Hazel;

namespace NextShip.RPC;

[HarmonyPatch]
public class RPCPatch
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    public class RPCHandlerPatch
    {
        private static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            /*RPCUtils.StartRPC(callId, reader);*/
        }

        private static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] byte callId,
            [HarmonyArgument(1)] MessageReader reader)
        {
            return RPCCheck.CheckRpc(__instance, callId, reader);
        }
    }
}