using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.MethodTooLong
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration), HighlightingTypes = new []
    {
        typeof(MethodTooLongHighlighting)
    })]
    public class MethodTooLongCheckVb : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxStatements = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumMethodStatements);
            var maxDeclarations = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumDeclarationsInMethod);

            var statementCount = element.CountChildren<IStatement>();
            if (statementCount <= maxStatements)
            {
                // Only look in the method body for declarations, otherwise we see
                // parameters + type parameters. We can ignore arrow expressions, as
                // they must be a single expression and won't have declarations
                var declarationCount = element.Block?.CountChildren<IDeclaration>() ?? 0;
                if (declarationCount <= maxDeclarations)
                    return;
            }

            var highlighting = new MethodTooLongHighlighting(element.GetNameDocumentRange());
            consumer.AddHighlighting(highlighting);
        }
    }
}