using HarmonyLib;

namespace NextShip.Roles;

public partial class RoleManager
{
    public void Restore()
    {
        Assigner.Restore();
        
        AllRoleBases.Do(n => n.Dispose());
        AllRoleBases.Clear();
    }

    public void OnGameEnd(AmongUsClient __instance, EndGameResult endGameResult)
    {
        Restore();
    }
}