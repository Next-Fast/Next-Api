using Hazel;

namespace NextShip.Api.RPCs;

#nullable enable
public class FastRpcWriter(MessageWriter? writer)
{
    private byte CallId;

    private int msgCount;

    private SendOption Option = SendOption.None;

    private int SendTargetId;

    private List<int> targetIds = new ();

    private uint targetObjectId;

    private FastRpcWriter() : this(MessageWriter.Get())
    {
    }

    private FastRpcWriter(SendOption option) : this(MessageWriter.Get(option))
    {
    }

    public static FastRpcWriter StartNew()
    {
        return new FastRpcWriter();
    }

    public static FastRpcWriter StartNew(byte call, SendOption option = SendOption.None)
    {
        return new FastRpcWriter(
            AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer!.PlayerId, call, option));
    }

    public void StartSendAllRPCWriter()
    {
        Clear();
        writer = MessageWriter.Get(Option);
        StartDataAllMessage();
        StartRPCMessage();
    }

    public void StartSendToPlayerRPCWriter()
    {
        Clear();
        writer = MessageWriter.Get(Option);
        StartDataToPlayerMessage();
        StartRPCMessage();
    }

    private void StartNextContactWriter(params PlayerControl[] targetPlayers)
    {
        Clear();

        writer = MessageWriter.Get(Option = SendOption.Reliable);

        StartMessage((byte)MessageFlags.NextContact);

        Write(PlayerControl.LocalPlayer.PlayerId);

        var players = targetPlayers.ToList();
        players.Remove(PlayerControl.LocalPlayer);

        WritePacked(players.Count);

        if (players.Count == PlayerControl.AllPlayerControls.Count - 1)
            return;

        foreach (var player in targetPlayers) Write(player.PlayerId);
    }

    public void SetSendOption(SendOption option)
    {
        Option = option;
    }

    public void SetTargetObjectId(uint id)
    {
        targetObjectId = id;
    }

    public void SetRpcCallId(byte id)
    {
        CallId = id;
    }

    public void SetTargetId(int id)
    {
        SendTargetId = id;
    }

    public void Set(SendOption option = SendOption.None, byte callId = byte.MaxValue, int targetId = -1,
        uint? objId = null)
    {
        Option = option;

        if (callId != byte.MaxValue)
            CallId = callId;

        if (targetId != -1)
            SendTargetId = targetId;

        if (objId != null)
            targetObjectId = (uint)objId;
    }

    public void Clear()
    {
        if (writer == null) return;
        Recycle();
        writer = null;
    }

    public void Write(bool value)
    {
        writer?.Write(value);
    }

    public void Write(int value)
    {
        writer?.Write(value);
    }

    public void Write(float value)
    {
        writer?.Write(value);
    }

    public void Write(string value)
    {
        writer?.Write(value);
    }

    public void Write(byte value)
    {
        writer?.Write(value);
    }

    public void WritePacked(int value)
    {
        writer?.WritePacked(value);
    }

    public void WritePacked(uint value)
    {
        writer?.WritePacked(value);
    }

    private void StartDataAllMessage()
    {
        StartMessage((byte)MessageFlags.DataAll);
        Write(AmongUsClient.Instance.GameId);
    }

    private void StartDataToPlayerMessage()
    {
        StartMessage((byte)MessageFlags.DataToPlayer);
        Write(AmongUsClient.Instance.GameId);
        WritePacked(SendTargetId);
    }

    private void StartRPCMessage()
    {
        StartMessage((byte)DataFlags.Rpc);
        WritePacked(targetObjectId);
        Write(CallId);
    }

    public void StartMessage(byte flag)
    {
        writer?.StartMessage(flag);
        msgCount++;
    }

    public void EndMessage()
    {
        writer?.EndMessage();
        msgCount--;
    }

    public void EndAllMessage()
    {
        while (msgCount > 0) EndMessage();
    }

    public void Recycle()
    {
        writer?.Recycle();
    }

    public void RPCSend()
    {
        EndAllMessage();
        AmongUsClient.Instance.SendOrDisconnect(writer);
        Recycle();
    }

    public void Finish()
    {
        AmongUsClient.Instance.FinishRpcImmediately(writer);
    }

    public void Send()
    {
        AmongUsClient.Instance.connection.Send(writer);
    }
}