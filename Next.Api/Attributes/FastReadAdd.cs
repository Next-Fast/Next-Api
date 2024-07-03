namespace Next.Api.Attributes;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class FastReadAdd(byte callId) : Attribute
{
    public readonly byte CallId = callId;
}