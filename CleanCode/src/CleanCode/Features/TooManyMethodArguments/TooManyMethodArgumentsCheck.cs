using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.TooManyMethodArguments
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration), HighlightingTypes = new []
    {
        typeof(TooManyArgumentsHighlighting)
    })]
    public class TooManyMethodArgumentsCheck : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxParameters = data.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsMaximum);
            var parameterDeclarations = element.ParameterDeclarations;

            if (parameterDeclarations.Count > maxParameters)
            {
                var highlighting = new TooManyArgumentsHighlighting(Warnings.TooManyMethodArguments,
                    element.GetNameDocumentRange());
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}