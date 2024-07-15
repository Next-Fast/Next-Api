using InnerNet;

namespace Next.Api.Bases;

public record NextInfo
{
    public float BodySpeed;

    public ClientData ClientData;

    public int clientId;

    public int ColorId;

    public string FriendCode;

    public float GhostSpeed;

    public bool IsHost;

    public bool IsLocal;

    public PlayerControl PlayerControl;

    public byte PlayerId;

    public NetworkedPlayerInfo PlayerInfo;

    public uint PlayerLevel;

    public string PlayerName;

    public PlayerPhysics PlayerPhysics;

    public string PUID;
}