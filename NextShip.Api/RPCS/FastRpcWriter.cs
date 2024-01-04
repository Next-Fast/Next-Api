using Hazel;

namespace NextShip.Api.RPCs;

#nullable enable
public class FastRpcWriter(MessageWriter? writer)
{
    private FastRpcWriter() : this(MessageWriter.Get()) { }
    
    private FastRpcWriter(SendOption option) : this(MessageWriter.Get(option)) { }

    private List<int> targetIds;

    private int targetObjectId;

    private SendOption Option = SendOption.None;

    private int msgCount = 0;

    private byte CallId;
    
    public static FastRpcWriter StartNew() =>
        new FastRpcWriter();

    public static FastRpcWriter StartNew(byte call, SendOption option = SendOption.None) =>
        new(AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer!.PlayerId, call, option));

    public void StartSendAllRPCWriter()
    {
        Clear();
        writer = MessageWriter.Get(Option);
        StartDataAllMessage();
        StartRPCMessage();
    }

    public void Set(SendOption option) => Option = option;

    public void SetTargetObjectId(int id) => targetObjectId = id;

    public void SetRpcCallId(byte id) => CallId = id;

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
        WritePacked(targetIds[0]);
        targetIds.RemoveAt(0);
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
        while (msgCount > 0)
        {
            EndMessage();
        }
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