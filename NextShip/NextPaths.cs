using System.Collections;
using NextShip.Api.Attributes;
using NextShip.Api.Enums;

namespace NextShip;

[Load(LoadMode.PreLoad)]
public class NextPaths
{
    [Load]
    public static IEnumerator PreLoad()
    {
        yield return null;
    }
}