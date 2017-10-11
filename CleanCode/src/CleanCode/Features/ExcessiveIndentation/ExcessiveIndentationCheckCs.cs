using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ExcessiveIndentation
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration),
        HighlightingTypes = new[]
        {
            typeof(ExcessiveIndentHighlighting)
        })]
    public class ExcessiveIndentationCheckCs : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxIndentation = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumIndentationDepth);

            var childrenDepth = element.GetChildrenDepth();
            if (childrenDepth > maxIndentation)
            {
                var documentRange = element.GetNameDocumentRange();
                var highlighting = new ExcessiveIndentHighlighting(documentRange, maxIndentation, childrenDepth);
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}