using System;
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
    public static void Create(byte rpc, byte[] bytes = null, int[] ints = null, float[] floats = null, bool[] bools = null, string[] strings = null)
    {
        MessageWriter rpcStart = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, rpc, SendOption.Reliable);
        if (bytes != null) { foreach (var b in bytes) { rpcStart.Write(b); } }

        if (ints != null) { foreach (var i in ints) { rpcStart.Write(i); } }

        if (floats != null) { foreach (var f in floats) { rpcStart.Write(f); } }

        if (bools != null) { foreach (var bo in bools) { rpcStart.Write(bo); } }

        if (strings != null) { foreach (var str in strings) { rpcStart.Write(str); } }

//        SendValueLength(bytes.Length, ints.Length, bools.Length, floats.Length, strings.Length);

        AmongUsClient.Instance.FinishRpcImmediately(rpcStart);
    }

    public static void ReadValue(MessageReader reader)
    {
        for (var b = 0; b < ReadRPCValue.byteL; b++)
        {
            byte ByteValue = reader.ReadByte();
            ReadRPCValue.bytes[b] = ByteValue;
        }

        for (var i = 0; i < ReadRPCValue.intL; i++)
        {
            int IntValue = reader.ReadInt32();
            ReadRPCValue.ints[i] = IntValue;
        }

        for (var bo = 0; bo < ReadRPCValue.boolL; bo++)
        {
            bool BoolValue = reader.ReadBoolean();
            ReadRPCValue.bools[bo] = BoolValue;
        }

        for (var f = 0; f < ReadRPCValue.floatL; f++)
        {
            float FloatValue = reader.ReadSingle();
            ReadRPCValue.floats[f] = FloatValue;
        }

        for (var s = 0; s < ReadRPCValue.stringL; s++)
        {
            string StringValue = reader.ReadString();
            ReadRPCValue.strings[s] = StringValue;
        }
    }

    public static void ReadValueLength(MessageReader reader)
    {
        ReadRPCValue.ClearAll();
        for (int length = reader.ReadInt32(); length == 0; length--)
        {
            byte type = reader.ReadByte();
            switch (type)
            {
                case (byte)ReadType.Byte:
                    ReadRPCValue.byteL = reader.ReadInt32();
                    break;

                case (byte)ReadType.Int:
                    ReadRPCValue.intL = reader.ReadInt32();
                    break;

                case (byte)ReadType.Bool:
                    ReadRPCValue.boolL = reader.ReadInt32();
                    break;

                case (byte)ReadType.Float:
                    ReadRPCValue.floatL = reader.ReadInt32();
                    break;

                case (byte)ReadType.String:
                    ReadRPCValue.stringL = reader.ReadInt32();
                    break;
            }
        }
        ReadValue(reader);
    }

    public static void SendValueLength(int byteL = 0, int intL = 0, int boolL = 0, int FloatL = 0, int StringL = 0)
    {
        MessageWriter rpcStart = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.RpcValueHandshake, SendOption.Reliable);
        int Length = 0;
        if (byteL != 0) Length++;
        if (intL != 0) Length++;
        if (boolL != 0) Length++;
        if (FloatL != 0) Length++;
        if (StringL != 0) Length++;
        rpcStart.Write(Length);

        if (byteL != 0)
        {
            rpcStart.Write((byte)ReadType.Byte);
            rpcStart.Write(byteL);
        }

        if (intL != 0)
        {
            rpcStart.Write((byte)ReadType.Int);
            rpcStart.Write(intL);
        }

        if (boolL != 0)
        {
            rpcStart.Write((byte)ReadType.Bool);
            rpcStart.Write(boolL);
        }

        if (FloatL != 0)
        {
            rpcStart.Write((byte)ReadType.Float);
            rpcStart.Write(FloatL);
        }

        if (StringL != 0)
        {
            rpcStart.Write((byte)ReadType.String);
            rpcStart.Write(StringL);
        }
        AmongUsClient.Instance.FinishRpcImmediately(rpcStart);
    }

    public static void StartRPC(byte rpc, MessageReader reader = null, byte[] bytes = null, string[] strings = null, bool[] bools = null )
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

            case (byte)CustomRPC.RestoreRole :
                byte RestoreRoleId = reader.ReadByte();
                RPCProcedure.RestoreRole(RestoreRoleId);
                break;

            case (byte)CustomRPC.RestorePlayerLook :
                RPCProcedure.RestorePlayerLook();
                break;

            case (byte)CustomRPC.HistorySynchronization:
                RPCProcedure.HistorySynchronization(reader.ReadByte(), reader.ReadInt32(), reader.ReadByte(), reader.ReadByte());
                break;

            case (byte)CustomRPC.ChangeRole :
                RPCProcedure.ChangeRole(reader.ReadByte(), reader.ReadByte());
                break;

            case (byte)CustomRPC.Camouflager :
                RPCProcedure.Camouflager();
                break;

            case (byte)CustomRPC.Illusory :
                RPCProcedure.Illusory();
                break;

            case (byte)CustomRPC.SchrodingerSCatTeamChange:
                RPCProcedure.SchrodingerSCatTeamChange(reader.ReadByte());
                break;

/*             case (byte)CustomRPC.LoverSendChat:
                PlayerControl sendChatPlayer = Helpers.GetPlayerForId(reader.ReadByte());
                string ChatText = reader.ReadString();
                RPCProcedure.LoverSendChat(sendChatPlayer, ChatText);
                break; */
        }
    }
}
public static class ReadRPCValue
{
    public static byte[] bytes;
    public static int[] ints;
    public static bool[] bools;
    public static float[] floats;
    public static string[] strings;
    public static int byteL;
    public static int intL;
    public static int boolL;
    public static int floatL;
    public static int stringL;
    public static void ClearAll()
    {
        bytes = null;
        ints = null;
        bools = null;
        floats = null;
        strings = null;
        byteL = 0;
        intL = 0;
        boolL = 0;
        floatL = 0;
        stringL = 0;
    }
}
public enum ReadType
{
    Byte,
    Int,
    Bool,
    Float,
    String
}