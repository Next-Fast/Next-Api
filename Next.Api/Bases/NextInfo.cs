using InnerNet;

namespace Next.Api.Bases;

public record NextInfo(
    float BodySpeed,
    ClientData ClientData,
    int clientId,
    int ColorId,
    string FriendCode,
    float GhostSpeed,
    bool IsHost,
    bool IsLocal,
    PlayerControl PlayerControl,
    byte PlayerId,
    NetworkedPlayerInfo PlayerInfo,
    uint PlayerLevel,
    string PlayerName,
    PlayerPhysics PlayerPhysics,
    string PUID)
{
    public float BodySpeed = BodySpeed;

    public ClientData ClientData = ClientData;

    public int clientId = clientId;

    public int ColorId = ColorId;

    public string FriendCode = FriendCode;

    public float GhostSpeed = GhostSpeed;

    public bool IsHost = IsHost;

    public bool IsLocal = IsLocal;

    public PlayerControl PlayerControl = PlayerControl;

    public byte PlayerId = PlayerId;

    public NetworkedPlayerInfo PlayerInfo = PlayerInfo;

    public uint PlayerLevel = PlayerLevel;

    public string PlayerName = PlayerName;

    public PlayerPhysics PlayerPhysics = PlayerPhysics;

    public string PUID = PUID;
}