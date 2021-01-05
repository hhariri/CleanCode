using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.TooManyMethodArguments
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration), HighlightingTypes = new[]
    {
        typeof(TooManyArgumentsHighlighting)
    })]
    public class TooManyMethodArgumentsCheckCs : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxParameters = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumMethodParameters);
            var parameterDeclarations = element.ParameterDeclarations;

            var parameterCount = parameterDeclarations.Count;
            if (parameterCount > maxParameters)
            {
                var highlighting = new TooManyArgumentsHighlighting(element.GetNameDocumentRange(), maxParameters, parameterCount);
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}