using HarmonyLib;
using Hazel;

namespace NextShip.RPC;

public class FastRPC
{
    public class FastWriter
    {
        private readonly MessageWriter _writer;

        public FastWriter()
        {
            _writer = MessageWriter.Get();
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
}