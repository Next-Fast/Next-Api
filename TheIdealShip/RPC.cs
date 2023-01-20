using System;
using HarmonyLib;
using Hazel;
using TheIdealShip.Utilities;
using TheIdealShip.Roles;
using static TheIdealShip.Roles.Role;
using static TheIdealShip.HudManagerStartPatch;

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
        setDead,

        // Role 职业相关
        SheriffKill,
    }

    public static class RPCProcedure
    {
        public static void ResetVariables()
        {
            clearAndReloadRoles();
            setCustomButtonCooldowns();
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

        public static void setDead(byte id, bool isDead)
        {
            foreach (var player in CachedPlayer.AllPlayers)
            {
                if (player.PlayerId == id)
                {
                    player.Data.IsDead = isDead;
                }
            }
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

                case (byte)CustomRPC.setDead:
                    byte id = reader.ReadByte();
                    bool isDead = reader.ReadBoolean();
                    RPCProcedure.setDead(id, isDead);
                    break;

                case (byte)CustomRPC.SheriffKill:
                    byte targetId = reader.ReadByte();
                    RPCProcedure.SheriffKill(targetId);
                    break;
            }
        }
    }
}