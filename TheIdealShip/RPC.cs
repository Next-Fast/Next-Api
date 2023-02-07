using System;
using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using TheIdealShip.Modules;
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
        RestoreRole,
        ChangeRole,
        customrpc,

        // Role 职业相关
        SheriffKill,
        Camouflager,
    }

    public static class RPCHelpers
    {
        public static void Create(byte rpc, byte[] bytes = null, int[] ints = null, float[] floats = null, bool[] bools = null)
        {
            MessageWriter rpcStart =  AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, rpc, SendOption.Reliable);
            if (bytes != null) { foreach (var b in bytes) { rpcStart.Write(b); } }

            if (ints != null) { foreach (var i in ints) { rpcStart.Write(i); } }

            if (floats != null) { foreach (var f in floats) { rpcStart.Write(f); } }

            if (bools != null) { foreach (var bo in bools) { rpcStart.Write(bo); } }
            
            AmongUsClient.Instance.FinishRpcImmediately(rpcStart);
        }
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
            for (int i = 0; i < numberOfRoles; i++)
            {
                byte playerId = (byte)reader.ReadPackedUInt32();
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
            var player = Helpers.GetPlayerForId(playerId);

            switch ((RoleId)roleId)
            {
                case RoleId.Sheriff:
                    Sheriff.sheriff = player;
                    break;
            }
        }

        public static void setModifier(byte modifierId, byte playerId, byte flag)
        {
            var player = Helpers.GetPlayerForId(playerId);
            switch ((RoleId)modifierId)
            {
                case RoleId.Flash:
                    Flash.flash = player;
                    break;
            }
        }

        public static void setDead(byte id, bool isDead)
        {
            var player = Helpers.GetPlayerForId(id);
            player.Data.IsDead = isDead;
        }

        public static void SheriffKill(byte targetId)
        {
            var player = Helpers.GetPlayerForId(targetId);
            Sheriff.sheriff.MurderPlayer(player);
        }

        public static void RestoreRole(byte id)
        {
            switch ((RoleId)id)
            {
                case RoleId.Sheriff:
                    Sheriff.sheriff = null;
                    break;
                case RoleId.Flash:
                    Flash.flash = null;
                    break;
                case RoleId.Jester:
                    Jester.jester = null;
                    break;
            }
        }

        public static void ChangeRole(byte playerId, byte targetRoleId)
        {
            var player = Helpers.GetPlayerForId(playerId);
            var info = RoleHelpers.GetRoleInfo(player);
            RestoreRole((byte)info.roleId);
            setRole(targetRoleId, playerId);
        }

        public static void Camouflager(bool isRestore)
        {
            foreach (var player in CachedPlayer.AllPlayers)
            {
                if (!isRestore)
                {
                    Helpers.setLook(player, "", 6, "", "", "", "");
                }
                else
                {
                    Helpers.setDefaultLook(player);
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
                case (byte)CustomRPC.RestoreRole :
                    byte RestoreRoleId = reader.ReadByte();
                    RPCProcedure.RestoreRole(RestoreRoleId);
                    break;
                case (byte)CustomRPC.ChangeRole :
                    byte ChangeRolePlayerId = reader.ReadByte();
                    byte ChangeRoleTargetRoleId = reader.ReadByte();
                    RPCProcedure.ChangeRole(ChangeRolePlayerId, ChangeRoleTargetRoleId);
                    break;
                case (byte)CustomRPC.Camouflager :
                    bool CamouflagerBool = reader.ReadBoolean();
                    RPCProcedure.Camouflager(CamouflagerBool);
                    break;
                case (byte)CustomRPC.customrpc:
                    reader.Recycle();
                    break;
            }
        }
    }
}