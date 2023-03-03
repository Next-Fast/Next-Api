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
            // RPCHelpers.StartRPC(packetId, reader);
            switch (packetId)
            {
                case (byte)CustomRPC.ResetVariables:
                    RPCProcedure.ResetVariables();
                    break;

                case (byte)CustomRPC.WorkaroundSetRoles:
                    RPCProcedure.WorkaroundSetRoles(reader.ReadByte(), reader);
                    break;

                case (byte)CustomRPC.SetRole:
                    byte roleId = reader.ReadByte();
                    byte playerId = reader.ReadByte();
                    RPCProcedure.setRole(roleId, playerId);
                    break;

                case (byte)CustomRPC.SetModifier:
                    byte modifierId = reader.ReadByte();
                    byte pId = reader.ReadByte();
                    byte flag = reader.ReadByte();
                    RPCProcedure.setModifier(modifierId, pId, flag);
                    break;

                case (byte)CustomRPC.setDead:
                    byte id = reader.ReadByte();
                    bool isDead = reader.ReadBoolean();
                    RPCProcedure.setDead(id, isDead);
                    break;

                case (byte)CustomRPC.SheriffKill:
                    byte targetId = reader.ReadByte();
                    RPCProcedure.SheriffKill(targetId);
                    break;

                case (byte)CustomRPC.RestoreRole:
                    byte RestoreRoleId = reader.ReadByte();
                    RPCProcedure.RestoreRole(RestoreRoleId);
                    break;

                case (byte)CustomRPC.RestorePlayerLook:
                    RPCProcedure.RestorePlayerLook();
                    break;

                case (byte)CustomRPC.HistorySynchronization:
                    RPCProcedure.HistorySynchronization(reader.ReadByte(), reader.ReadInt32(), reader.ReadByte(), reader.ReadByte());
                    break;

                case (byte)CustomRPC.ChangeRole:
                    byte ChangeRolePlayerId = reader.ReadByte();
                    byte ChangeRoleTargetRoleId = reader.ReadByte();
                    RPCProcedure.ChangeRole(ChangeRolePlayerId, ChangeRoleTargetRoleId);
                    break;

                case (byte)CustomRPC.Camouflager:
                    RPCProcedure.Camouflager();
                    break;

                case (byte)CustomRPC.Illusory:
                    RPCProcedure.Illusory();
                    break;

                case (byte)CustomRPC.SchrodingerSCatTeamChange:
                    byte SchrodingerSCatTeam = reader.ReadByte();
                    RPCProcedure.SchrodingerSCatTeamChange(SchrodingerSCatTeam);
                    break;

                case (byte)CustomRPC.customrpc:
                    reader.Recycle();
                    break;
            }
        }
    }
}