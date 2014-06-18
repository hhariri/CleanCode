using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ExcessiveIndentation
{
    public class ExcessiveIndentationCheck : SimpleCheck<IMethodDeclaration, int>
    {
        public ExcessiveIndentationCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration statement, IHighlightingConsumer consumer)
        {
            var maxIndentation = Threshold;
            var depth = statement.GetChildrenDepth();

            if (depth > maxIndentation)
            {
                var highlighting = new Highlighting(Warnings.ExcessiveDepth);
                consumer.AddHighlighting(highlighting, statement.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ExcessiveIndentationMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ExcessiveIndentationEnabled); }
        }
    }
}