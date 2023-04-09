using HarmonyLib;
using Hazel;

namespace TheIdealShip;

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
    }
}