namespace Next.Api.Extension;

public static class CrowdedExtension
{
    private static bool Started;

    public static void Use()
    {
        if (Started) return;

        Started = true;
    }
}