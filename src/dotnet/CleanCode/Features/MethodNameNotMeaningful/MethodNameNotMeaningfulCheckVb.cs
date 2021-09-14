using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.MethodNameNotMeaningful
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration), HighlightingTypes = new[]
    {
        typeof(MethodNameNotMeaningfulHighlighting)
    })]
    public class MethodNameNotMeaningfulCheckVb : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.Name == null)
                return;

            var minimumMethodNameLength = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MinimumMeaningfulMethodNameLength);
            var name = element.Name.GetText();

            if (name.Length < minimumMethodNameLength)
            {
                var documentRange = element.GetNameDocumentRange();
                var highlighting = new MethodNameNotMeaningfulHighlighting(documentRange);
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}