using Hazel;
using TheIdealShip.Utilities;

namespace TheIdealShip;
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
        MessageWriter rpcStart = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, rpc, SendOption.Reliable);
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