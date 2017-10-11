using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB.Tree;

namespace CleanCode.Features.ClassTooBig
{
    [ElementProblemAnalyzer(typeof(IClassDeclaration), HighlightingTypes = new[]
    {
        typeof(ClassTooBigHighlighting)
    })]
    public class ClassTooBigCheckVb : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxLength = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumMethodsInClass);

            var statementCount = element.CountChildren<IMethodDeclaration>();
            if (statementCount > maxLength)
            {
                var declarationIdentifier = element.Name;
                var documentRange = declarationIdentifier.GetDocumentRange();
                var highlighting = new ClassTooBigHighlighting(documentRange, maxLength, statementCount);
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}