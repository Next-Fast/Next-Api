using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using InnerNet;

namespace NextShip.RPC;

public class FastRPC
{
    public readonly FastReader Reader;
    public readonly FastWriter Writer;

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

    [HarmonyPatch]
    public class FastReader
    {
        public static List<FastReader> AllReader = new();
        public MessageReader _Reader;

        public Action<byte, MessageReader> HandleRpc = null;
        public MessageReader ParentReader = new();
        private byte? TargetId;

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

        public void SetTarget(byte id)
        {
            TargetId = id;
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
            var HandleReader = MessageReader.Get(reader);
            if (reader.Tag == 2)
                AllReader.Do(n => n.ParentReader = HandleReader);
            try
            {
                var id = HandleReader.ReadByte();
                AllReader.Where(n => n.TargetId != null && n.TargetId == id && n.HandleRpc != null)
                    .Do(n => n.HandleRpc(id, HandleReader));
            }
            catch (Exception e)
            {
                Exception(e);
            }

            finally
            {
                reader.Recycle();
            }
        }
    }
}