namespace NextShip.ModUpdater;

public class ModUpdater : Updater
{
    public override Task Update(List<(string, Program.UpdateOption)> version)
    {
        return Task.CompletedTask;
    }
}