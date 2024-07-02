using System;
using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using NextShip.Api.Enums;

namespace NextShip.Patches;

[HarmonyPatch]
public static class RPCSyncPatch
{
    public static readonly List<DefRPC> AllDefRpcS = [];


    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.HandleGameDataInner))]
    [HarmonyPrefix]
    public static bool OnHandleGameDataInnerPreFix(AmongUsClient __instance, [HarmonyArgument(0)] MessageReader reader)
    {
        var Has = false;
        var HandleReader = MessageReader.Get(reader);
        HandleReader.Position = 0;
        var tag = reader.Tag;
        if (tag != (byte)DataFlags.Rpc)
            return true;
        try
        {
            HandleReader.ReadPackedUInt32();
            var callId = HandleReader.ReadByte();
            Has = AllDefRpcS.Exists(n => n.HasRPC(callId));
            AllDefRpcS.Do(n =>
            {
                var read = MessageReader.Get(HandleReader);
                n.OnRPC(ref read);
                read.Recycle();
            });
        }
        catch (Exception e)
        {
            Exception(e);
        }

        finally
        {
            HandleReader.Recycle();
        }

        return !Has;
    }
}

public abstract class DefRPC
{
    protected DefRPC()
    {
        RPCSyncPatch.AllDefRpcS.Add(this);
    }

    public abstract bool HasRPC(byte rpcFlag);

    public abstract void OnRPC(ref MessageReader reader);
}