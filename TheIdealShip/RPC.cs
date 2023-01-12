using System;
using HarmonyLib;
using Hazel;
using TheIdealShip.Utilities;
using TheIdealShip.Roles;

namespace TheIdealShip
{
    enum CustomRPC
    {
        // Main 主要
        ResetVariables,
        ShareOptions,
        WorkaroundSetRoles,
        SetRole,
        SetModifier,

        // Role 职业相关
        SheriffKill,
    }

    public static class RPCProcedure
    {
        public static void ResetVariables()
        {

        }

        public static void WorkaroundSetRoles(byte numberOfRoles, MessageReader reader)
        {
            for (int i = 0;i < numberOfRoles; i++)
            {
                byte playerId = (byte) reader.ReadPackedUInt32();
                byte roleId = (byte)reader.ReadPackedUInt32();
                try
                {
                    setRole(roleId, playerId);
                }
                catch (Exception e)
                {
                    TheIdealShipPlugin.Logger.LogError("Error while deserializing roles: " + e.Message);
                }
            }
        }

        public static void setRole(byte roleId, byte playerId)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
            {
                if (player.PlayerId == playerId)
                {
                    switch((RoleId)roleId)
                    {
                        case RoleId.Sheriff:
                            Sheriff.sheriff = player;
                            break;
                    }
                }
            }
        }

        public static void setModifier(byte modifierId, byte playerId, byte flag)
        {

        }

        public static void SheriffKill(byte targetId)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
            {
                if (player.PlayerId == targetId)
                {
                    Sheriff.sheriff.MurderPlayer(player);
                    return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)]byte callId,[HarmonyArgument(1)]MessageReader reader)
        {
            byte packetId = callId;
            switch (packetId)
            {
                case (byte)CustomRPC.WorkaroundSetRoles:
                    RPCProcedure.WorkaroundSetRoles(reader.ReadByte(), reader);
                    break;

                case (byte)CustomRPC.SetRole:
                    byte roleId = reader.ReadByte();
                    byte playerId = reader.ReadByte();
                    RPCProcedure.setRole(roleId, playerId);
                    break;

                case (byte)CustomRPC.SheriffKill:
                    byte targetId = reader.ReadByte();
                    RPCProcedure.SheriffKill(targetId);
                    break;
            }
        }
    }
}