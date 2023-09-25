using System.Collections.Generic;
using System.Net.Sockets;
using NextShip.Chat.Patches;
using NextShip.Utilities;

namespace NextShip.Chat;

public sealed class VoiceChatManager
{
    private static VoiceChatManager Instance;
    public PlayerControl LocalPlayer = CachedPlayer.LocalPlayer;

    public static VoiceChatManager Get() => Instance ??= new VoiceChatManager();

    public List<VoiceClient> AllClient = new();
    public VoiceConnectType VoiceConnectType;

    public TcpClient TcpClient;
    public UdpClient UdpClient;
}

public enum VoiceConnectType
{
    RPCConnect, // RPC连接
    ServerConnect, // 额外服务器连接
    LocalConnect // 本地连接
}