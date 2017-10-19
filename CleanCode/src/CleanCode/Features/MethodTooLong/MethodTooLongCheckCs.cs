using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.MethodTooLong
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration), HighlightingTypes = new[]
    {
        typeof(MethodTooLongHighlighting)
    })]
    public class MethodTooLongCheckCs : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var highlighting = GetHighlighting(element, data);
            if (highlighting != null)
                consumer.AddHighlighting(highlighting);
        }

        private IHighlighting GetHighlighting(IMethodDeclaration element, ElementProblemAnalyzerData data)
        {
            var maxStatements = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumMethodStatements);
            var maxDeclarations = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumDeclarationsInMethod);

            var highlighting = CheckStatementCount(element, maxStatements);
            highlighting = CheckDeclarationCount(element, maxDeclarations);

            return highlighting;
        }

        private static IHighlighting CheckStatementCount(IMethodDeclaration element, int maxStatements)
        {
            var statementCount = element.CountChildren<IStatement>();
            if (statementCount > maxStatements) return new MethodTooLongHighlighting(element.GetNameDocumentRange(), maxStatements, statementCount);
            return null;
        }

        private static IHighlighting CheckDeclarationCount(IMethodDeclaration element, int maxDeclarations)
        {
            // Only look in the method body for declarations, otherwise we see
            // parameters + type parameters. We can ignore arrow expressions, as
            // they must be a single expression and won't have declarations
            var declarationCount = element.Body?.CountChildren<IDeclaration>() ?? 0;
            if (declarationCount > maxDeclarations) return new MethodTooManyDeclarationsHighlighting(element.GetNameDocumentRange(), maxDeclarations, declarationCount);
            return null;
        }
    }
}