using System.Collections.Generic;
using System.Net.Sockets;

namespace NextShip.Chat;

public sealed class VoiceChatManager
{
    private static VoiceChatManager Instance;

    public List<VoiceClient> AllClient = new();
    public PlayerControl LocalPlayer = CachedPlayer.LocalPlayer;

    public TcpClient TcpClient;
    public UdpClient UdpClient;
    public VoiceConnectType VoiceConnectType;

    public static VoiceChatManager Get()
    {
        return Instance ??= new VoiceChatManager();
    }
}

public enum VoiceConnectType
{
    RPCConnect, // RPC连接
    ServerConnect, // 额外服务器连接
    LocalConnect // 本地连接
}