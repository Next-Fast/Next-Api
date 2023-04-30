using HarmonyLib;
using Hazel;

namespace TheIdealShip.RPC;

[HarmonyPatch]
public class RPCPatch
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            byte packetId = callId;
            RPCHelpers.StartRPC(packetId, reader);
        }

        static void Prefix(PlayerControl __instance,[HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {

        }
    }
}