using System;
using HarmonyLib;
using Hazel;

namespace TheIdealShip
{
    enum CustomRPC
    {
        // Main 主要
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

        public static void setRole(byte roleId, byte playerId)
        {

        }

        public static void setModifier(byte modifierId, byte playerId, byte flag)
        {

        }
    }

    [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)]byte callId,[HarmonyArgument(1)]MessageReader reader)
        {

        }
    }
}