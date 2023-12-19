namespace NextShip.Api.Bases;

public record NextInfo
{
    public int clientId;

    public string FriendCode;

    public PlayerControl PlayerControl;

    public GameData.PlayerInfo PlayerInfo;

    public string PUID;

    public RoleBase Role;

    public float GhostSpeed;

    public float BodySpeed;
}