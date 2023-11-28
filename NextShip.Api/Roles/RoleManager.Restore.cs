using HarmonyLib;

namespace NextShip.Api.Roles;

public partial class RoleManager
{
    public void OnGameEnd(AmongUsClient __instance, EndGameResult endGameResult)
    {
        Restore();
    }

    public void Restore()
    {
        Assigner.Restore();

        AllRoleBases.Do(n => n.Dispose());
        AllRoleBases.Clear();
    }
}