namespace Next.Api.Roles;

public static class RoleEnum
{
    public static readonly Random rnd = new((int)DateTime.Now.Ticks);
}

public enum RoleId
{
    // Crewmate 船员
    Crewmate,
    Sheriff,
    Postman,

    // Impostor 内鬼
    Impostor,
    Camouflager,
    Illusory,

    // Neutral 中立
    Jester,
    SchrodingersCats,

    // Modifier 附加
    Flash,
    Lover,
    none
}

public enum RoleType
{
    MainRole,
    ModifierRole,
    NotRole
}

public enum RoleTeam
{
    Crewmate,
    Impostor,
    Neutral,
    none
}