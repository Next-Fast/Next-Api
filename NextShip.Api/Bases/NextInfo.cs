using InnerNet;
using NextShip.Api.Interfaces;

namespace NextShip.Api.Bases;

public record NextInfo
{
    public float BodySpeed;
    
    public int clientId;

    public byte PlayerId;

    public string FriendCode;

    public string PlayerName;
    
    public uint PlayerLevel;

    public int ColorId;
    
    public float GhostSpeed;

    public PlayerControl PlayerControl;

    public ClientData ClientData;

    public GameData.PlayerInfo PlayerInfo;

    public string PUID;

    public RoleBase RoleBase;

    public IRole Role;

    public bool IsHost;

    public bool IsLocal;
}