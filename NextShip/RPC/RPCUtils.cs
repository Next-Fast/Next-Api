using HarmonyLib;
using Hazel;
using NextShip.Utilities;

namespace NextShip.RPC;

public class RPCUtils
{
    public static void Create(byte rpc, byte[] bytes = null, int[] ints = null, float[] floats = null,
        bool[] bools = null, string[] strings = null)
    {
        var rpcStart = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, rpc,
            SendOption.Reliable);
        if (bytes != null)
            foreach (var b in bytes)
                rpcStart.Write(b);

        if (ints != null)
            foreach (var i in ints)
                rpcStart.Write(i);

        if (floats != null)
            foreach (var f in floats)
                rpcStart.Write(f);

        if (bools != null)
            foreach (var bo in bools)
                rpcStart.Write(bo);

        if (strings != null)
            foreach (var str in strings)
                rpcStart.Write(str);

        SendValueLength(bytes.Length, ints.Length, bools.Length, floats.Length, strings.Length);

        AmongUsClient.Instance.FinishRpcImmediately(rpcStart);
    }

    public static void ReadValue(MessageReader reader)
    {
        for (var b = 0; b < ReadRPCValue.byteL; b++)
        {
            var ByteValue = reader.ReadByte();
            ReadRPCValue.bytes[b] = ByteValue;
        }

        for (var i = 0; i < ReadRPCValue.intL; i++)
        {
            var IntValue = reader.ReadInt32();
            ReadRPCValue.ints[i] = IntValue;
        }

        for (var bo = 0; bo < ReadRPCValue.boolL; bo++)
        {
            var BoolValue = reader.ReadBoolean();
            ReadRPCValue.bools[bo] = BoolValue;
        }

        for (var f = 0; f < ReadRPCValue.floatL; f++)
        {
            var FloatValue = reader.ReadSingle();
            ReadRPCValue.floats[f] = FloatValue;
        }

        for (var s = 0; s < ReadRPCValue.stringL; s++)
        {
            var StringValue = reader.ReadString();
            ReadRPCValue.strings[s] = StringValue;
        }
    }

    public static void ReadValueLength(MessageReader reader)
    {
        ReadRPCValue.ClearAll();
        for (var length = reader.ReadInt32(); length == 0; length--)
        {
            var type = reader.ReadByte();
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
        var rpcStart = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
            (byte)CustomRPC.RpcValueHandshake, SendOption.Reliable);
        var Length = 0;
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

public class FastWriter
{
    private MessageWriter _writer;

    public FastWriter()
    {
        _writer ??= MessageWriter.Get();
    }

    public FastWriter(MessageWriter writer) => _writer = writer;

    public void Write(bool value) => _writer.Write(value);
    public void Write(int value) => _writer.Write(value);
    public void Write(float value) => _writer.Write(value);
    public void Write(string value) => _writer.Write(value);
    public void Write(byte value) => _writer.Write(value);
}

public class FastReader
{
    public MessageReader ParentReader = new MessageReader();
    public MessageReader _Reader;

    public FastReader()
    {
        _Reader = MessageReader.Get(ParentReader);
    }

    public FastReader(MessageReader reader)
    {
        _Reader = reader;
    }

    public bool ReadBool() => _Reader.ReadBoolean();
    public int ReadInt() => _Reader.ReadInt32();
    public float ReadFloat() => _Reader.ReadSingle();
    public string ReadString() => _Reader.ReadString();
    public byte ReadByte() => _Reader.ReadByte();
    
    [HarmonyPatch(typeof(InnerNet.InnerNetClient), nameof(InnerNet.InnerNetClient.HandleGameDataInner)), HarmonyPostfix]
    public void InnerNet_ReaderPath([HarmonyArgument(0)]MessageReader reader)
    {
        if (reader.Tag == 2) ParentReader = reader;
    }
}