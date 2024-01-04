using Hazel;

namespace NextShip.Api.RPCs;

public class FastRpcWriter(MessageWriter writer)
{
    private FastRpcWriter() : this(MessageWriter.Get()) { }
    
    public static FastRpcWriter StartNew() =>
        new FastRpcWriter();

    public static FastRpcWriter StartNew(byte call, SendOption option = SendOption.None) =>
        new(AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer!.PlayerId, call, option));
    
    public void Write(bool value)
    {
        writer.Write(value);
    }

    public void Write(int value)
    {
        writer.Write(value);
    }

    public void Write(float value)
    {
        writer.Write(value);
    }

    public void Write(string value)
    {
        writer.Write(value);
    }

    public void Write(byte value)
    {
        writer.Write(value);
    }

    public void Finish()
    {
        AmongUsClient.Instance.FinishRpcImmediately(writer);
    }
    
    public void Send()
    {
        AmongUsClient.Instance.connection.SendDisconnect(writer);
    }
}