namespace NextShip.Api.Bases;

public record NextInfo
{
    public int clientId;

    public RoleBase Role;

    public PlayerControl PlayerControl;

    public GameData.PlayerInfo PlayerInfo;

    public string PUID;

    public string FriendCode;
}