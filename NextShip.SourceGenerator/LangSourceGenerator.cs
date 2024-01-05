using Microsoft.CodeAnalysis;

namespace NextShip.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class LangSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
    }
}