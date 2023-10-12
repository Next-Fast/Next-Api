namespace NextShip.ModUpdater;

public class BepInExUpdater : Updater
{
    public override Task Update(List<(string, Program.UpdateOption)> version)
    {
        return Task.CompletedTask;
    }
}