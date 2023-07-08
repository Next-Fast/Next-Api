using HarmonyLib;
using Hazel;
using InnerNet;

namespace TheIdealShip.RPC;

[HarmonyPatch]
public class RPCPatch
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    public class RPCHandlerPatch
    {
        private static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            var packetId = callId;
            RPCHelpers.StartRPC(packetId, reader);
        }

        private static void Prefix(PlayerControl __instance, [HarmonyArgument(0)] byte callId,
            [HarmonyArgument(1)] MessageReader reader)
        {
        }
    }

    public class rpc : InnerNetObject
    {
        public override void HandleRpc(byte callId, MessageReader reader)
        {
        }
    }
}