using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using InnerNet;

namespace NextShip.RPC;

public class FastRPC
{
    public readonly FastWriter Writer;
    public readonly FastReader Reader;

    public FastRPC()
    {
        Writer = new FastWriter();
        Reader = new FastReader();
    }
    
    public class FastWriter
    {
        private readonly MessageWriter _writer;

        public FastWriter()
        {
            _writer = MessageWriter.Get();
        }

        public FastWriter(MessageWriter writer)
        {
            _writer = writer;
        }

        public void Write(bool value)
        {
            _writer.Write(value);
        }

        public void Write(int value)
        {
            _writer.Write(value);
        }

        public void Write(float value)
        {
            _writer.Write(value);
        }

        public void Write(string value)
        {
            _writer.Write(value);
        }

        public void Write(byte value)
        {
            _writer.Write(value);
        }
    }

    public class FastReader
    {
        public static List<FastReader> AllReader = new ();
        public MessageReader _Reader;
        public MessageReader ParentReader = new();

        public FastReader()
        {
            _Reader = MessageReader.Get(ParentReader);
            
            AllReader.Add(this);
        }

        public FastReader(MessageReader reader)
        {
            _Reader = reader;
            
            AllReader.Add(this);
        }

        public bool ReadBool()
        {
            return _Reader.ReadBoolean();
        }

        public int ReadInt()
        {
            return _Reader.ReadInt32();
        }

        public float ReadFloat()
        {
            return _Reader.ReadSingle();
        }

        public string ReadString()
        {
            return _Reader.ReadString();
        }

        public byte ReadByte()
        {
            return _Reader.ReadByte();
        }

        [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.HandleGameDataInner))]
        [HarmonyPostfix]
        public static void InnerNet_ReaderPath([HarmonyArgument(0)] MessageReader reader)
        {
            if (AllReader.Count <= 0) return;
            if (reader.Tag == 2) AllReader.Do(n => n.ParentReader = reader);
        }
    }
}