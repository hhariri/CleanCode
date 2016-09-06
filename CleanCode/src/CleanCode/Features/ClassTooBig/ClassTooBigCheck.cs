using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ClassTooBig
{
    [ElementProblemAnalyzer(typeof(IClassDeclaration), HighlightingTypes = new []
    {
        typeof(ClassTooBigHighlighting)
    })]
    public class ClassTooBigCheck : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxLength = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumMethodsInClass);

            var statementCount = element.CountChildren<IMethodDeclaration>();
            if (statementCount > maxLength)
            {
                var declarationIdentifier = element.NameIdentifier;
                var documentRange = declarationIdentifier.GetDocumentRange();
                var highlighting = new ClassTooBigHighlighting(Warnings.ClassTooBig, documentRange);
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}