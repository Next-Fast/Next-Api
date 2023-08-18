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
            RPCHelpers.StartRPC(callId, reader);
        }

        private static void Prefix(PlayerControl __instance, [HarmonyArgument(0)] byte callId,
            [HarmonyArgument(1)] MessageReader reader)
        {
            RPCCheck.CheckRpc(__instance, callId, reader);
        }
    }
}

public class RPCHelpers
{
    public static void StartRPC(byte rpc, MessageReader reader)
    {
        switch (rpc)
        {
            case (byte)CustomRPC.ResetVariables:
                RPCProcedure.ResetVariables();
                break;

            case (byte)CustomRPC.WorkaroundSetRoles:
                RPCProcedure.WorkaroundSetRoles(reader.ReadByte(), reader);
                break;

            case (byte)CustomRPC.SetRole:
                RPCProcedure.setRole(reader.ReadByte(), reader.ReadByte());
                break;

            case (byte)CustomRPC.SetModifier:
                RPCProcedure.setModifier(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                break;

            case (byte)CustomRPC.setDead:
                RPCProcedure.setDead(reader.ReadByte(), reader.ReadBoolean());
                break;

            case (byte)CustomRPC.SheriffKill:
                RPCProcedure.SheriffKill(reader.ReadByte());
                break;

            case (byte)CustomRPC.RestoreRole:
                var RestoreRoleId = reader.ReadByte();
                RPCProcedure.RestoreRole(RestoreRoleId);
                break;

            case (byte)CustomRPC.RestorePlayerLook:
                RPCProcedure.RestorePlayerLook();
                break;

            case (byte)CustomRPC.ChangeRole:
                RPCProcedure.ChangeRole(reader.ReadByte(), reader.ReadByte());
                break;

            case (byte)CustomRPC.Camouflager:
                RPCProcedure.Camouflager();
                break;

            case (byte)CustomRPC.Illusory:
                RPCProcedure.Illusory();
                break;
        }
    }
}