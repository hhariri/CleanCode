using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.MethodTooLong
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration), HighlightingTypes = new []
    {
        typeof(MethodTooLongHighlighting)
    })]
    public class MethodTooLongCheck : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxLength = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MethodTooLongMaximum);

            var statementCount = element.CountChildren<IStatement>();
            if (statementCount > maxLength)
            {
                var highlighting = new MethodTooLongHighlighting(element.GetNameDocumentRange());
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}