using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class NextPatchManager : IPatchManager
{
    public static NextPatchManager _NextPatchManager = Main._Service.Get<NextPatchManager>();
}