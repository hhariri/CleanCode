using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ExcessiveIndentation
{
    public class ExcessiveIndentationCheck : MonoValueCheck<IMethodDeclaration, int>
    {
        public ExcessiveIndentationCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration constructorDeclaration, IHighlightingConsumer consumer)
        {
            var maxIndentation = Value;
            var depth = constructorDeclaration.GetChildrenDepth();

            if (depth > maxIndentation)
            {
                var documentRange = constructorDeclaration.GetNameDocumentRange();
                var highlighting = new Highlighting(Warnings.ExcessiveDepth, documentRange);
                consumer.AddHighlighting(highlighting);
            }
        }

        protected override int Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ExcessiveIndentationMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ExcessiveIndentationEnabled); }
        }
    }
}