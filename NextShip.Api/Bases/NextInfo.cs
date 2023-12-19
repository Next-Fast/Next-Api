namespace NextShip.Api.Bases;

public record NextInfo
{
    public float BodySpeed;
    public int clientId;

    public string FriendCode;

    public float GhostSpeed;

    public PlayerControl PlayerControl;

    public GameData.PlayerInfo PlayerInfo;

    public string PUID;

    public RoleBase Role;
}