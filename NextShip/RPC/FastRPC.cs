using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using InnerNet;

namespace NextShip.RPC;

public class FastRPC
{
    public readonly FastReader Reader = new();

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
        
    }
}