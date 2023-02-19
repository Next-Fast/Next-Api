using System;
using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using TheIdealShip.Modules;
using TheIdealShip.HistoryManager;
using TheIdealShip.Utilities;
using TheIdealShip.Roles;
using static TheIdealShip.Roles.Role;
using static  TheIdealShip.Helpers;
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
        RestorePlayerLook,
        customrpc,
        VersionShakehand,
        HistorySynchronization,

        // Role 职业相关
        SheriffKill,
        Camouflager,
        Illusory,
        SchrodingerSCatTeamChange
    }

    public static class RPCHelpers
    {
        // public static List<byte> OtherRPCBytes = new List<byte>();
        // public static Dictionary<byte, int> OtherRPCNumber = new Dictionary<byte, int>();
        //
        // public static bool AddToOtherRPCList(byte call)
        // {
        //     if (OtherRPCBytes.Contains(call))
        //     {
        //         OtherRPCNumber[call]++;
        //         return true;
        //     }
        //     else
        //     {
        //         OtherRPCBytes.Add(call);
        //         OtherRPCNumber.Add(call, 1);
        //         return false;
        //     }
        // }
        public static void Create(byte rpc, byte[] bytes = null, int[] ints = null, float[] floats = null, bool[] bools = null)
        {
            MessageWriter rpcStart =  AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, rpc, SendOption.Reliable);
            if (bytes != null) { foreach (var b in bytes) { rpcStart.Write(b); } }

            if (ints != null) { foreach (var i in ints) { rpcStart.Write(i); } }

            if (floats != null) { foreach (var f in floats) { rpcStart.Write(f); } }

            if (bools != null) { foreach (var bo in bools) { rpcStart.Write(bo); } }
            
            AmongUsClient.Instance.FinishRpcImmediately(rpcStart);
        }

        // public static void StartRPC(byte rpc, MessageReader reader = null)
        // {
        //     switch (rpc)
        //     {
        //         case (byte)CustomRPC.ResetVariables:
        //             RPCProcedure.ResetVariables();
        //             break;
        //
        //         case (byte)CustomRPC.WorkaroundSetRoles:
        //             RPCProcedure.WorkaroundSetRoles(reader.ReadByte(), reader);
        //             break;
        //
        //         case (byte)CustomRPC.SetRole:
        //             byte roleId = reader.ReadByte();
        //             byte playerId = reader.ReadByte();
        //             RPCProcedure.setRole(roleId, playerId);
        //             break;
        //
        //         case (byte)CustomRPC.SetModifier:
        //             byte modifierId = reader.ReadByte();
        //             byte pId = reader.ReadByte();
        //             byte flag = reader.ReadByte();
        //             RPCProcedure.setModifier(modifierId, pId, flag);
        //             break;
        //
        //         case (byte)CustomRPC.setDead:
        //             byte id = reader.ReadByte();
        //             bool isDead = reader.ReadBoolean();
        //             RPCProcedure.setDead(id, isDead);
        //             break;
        //
        //         case (byte)CustomRPC.SheriffKill:
        //             byte targetId = reader.ReadByte();
        //             RPCProcedure.SheriffKill(targetId);
        //             break;
        //         
        //         case (byte)CustomRPC.RestoreRole :
        //             byte RestoreRoleId = reader.ReadByte();
        //             RPCProcedure.RestoreRole(RestoreRoleId);
        //             break;
        //         
        //         case (byte)CustomRPC.RestorePlayerLook :
        //             RPCProcedure.RestorePlayerLook();
        //             break;
        //         
        //         case (byte)CustomRPC.ChangeRole :
        //             byte ChangeRolePlayerId = reader.ReadByte();
        //             byte ChangeRoleTargetRoleId = reader.ReadByte();
        //             RPCProcedure.ChangeRole(ChangeRolePlayerId, ChangeRoleTargetRoleId);
        //             break;
        //         
        //         case (byte)CustomRPC.Camouflager :
        //             RPCProcedure.Camouflager();
        //             break;
        //         
        //         case (byte)CustomRPC.Illusory :
        //             RPCProcedure.Illusory();
        //             break;
        //         
        //         case (byte)CustomRPC.customrpc:
        //             reader.Recycle();
        //             break;
        //     }
        // }
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
                
                case RoleId.Jester:
                    Jester.jester = player;
                    break;
                
                case RoleId.Camouflager:
                    Roles.Camouflager.camouflager = player;
                    break;
                
                case RoleId.Illusory:
                    Roles.Illusory.illusory = player;
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
                
                case RoleId.Lover:
                    if (flag == 0) { Lover.lover1 = player; } else { Lover.lover2 = player; }
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

        public static void RestorePlayerLook()
        {
            foreach (var player in CachedPlayer.AllPlayers)
            {
                player.PlayerControl.setDefaultLook();
            }
        }

        public static void HistorySynchronization(byte playerid, int number, byte role , byte team)
        {
            HistoryInfoManager.Add(Helpers.GetPlayerForId(playerid), (RoleInfo.RoleTeam)team, (RoleId)role, true,number);
        }

        public static void Camouflager()
        {
            foreach (var player in CachedPlayer.AllPlayers)
            {
                player.PlayerControl.setLook("", 6, "", "", "", "");
            }
        }

        public static void Illusory()
        {
            if (CachedPlayer.LocalPlayer.PlayerControl.Data.Role.IsImpostor) return;

            PlayerControl rndPlayer = CachedPlayer.AllPlayers[rnd.Next(0, CachedPlayer.AllPlayers.Count)].PlayerControl;
            foreach (var player in CachedPlayer.AllPlayers)
            {
                player.PlayerControl.setLook(rndPlayer.Data.PlayerName, rndPlayer.Data.DefaultOutfit.ColorId, rndPlayer.Data.DefaultOutfit.HatId, rndPlayer.Data.DefaultOutfit.VisorId, rndPlayer.Data.DefaultOutfit.SkinId, rndPlayer.Data.DefaultOutfit.PetId);
            }
        }

        public static void SchrodingerSCatTeamChange(byte team)
        {
            SchrodingersCat.team = (RoleInfo.RoleTeam)team;
        }
    }

    [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)]byte callId,[HarmonyArgument(1)]MessageReader reader)
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
                
                case (byte)CustomRPC.RestoreRole :
                    byte RestoreRoleId = reader.ReadByte();
                    RPCProcedure.RestoreRole(RestoreRoleId);
                    break;
                
                case (byte)CustomRPC.RestorePlayerLook :
                    RPCProcedure.RestorePlayerLook();
                    break;
                
                case (byte)CustomRPC.HistorySynchronization :
                    RPCProcedure.HistorySynchronization(reader.ReadByte(), reader.ReadInt32(), reader.ReadByte(), reader.ReadByte());
                    break;
                
                case (byte)CustomRPC.ChangeRole :
                    byte ChangeRolePlayerId = reader.ReadByte();
                    byte ChangeRoleTargetRoleId = reader.ReadByte();
                    RPCProcedure.ChangeRole(ChangeRolePlayerId, ChangeRoleTargetRoleId);
                    break;
                
                case (byte)CustomRPC.Camouflager :
                    RPCProcedure.Camouflager();
                    break;
                
                case (byte)CustomRPC.Illusory :
                    RPCProcedure.Illusory();
                    break;
                
                case (byte)CustomRPC.SchrodingerSCatTeamChange :
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