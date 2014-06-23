using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.MethodTooLong
{
    public class MethodTooLongCheck : SimpleCheck<IMethodDeclaration, int>
    {
        public MethodTooLongCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration statement, IHighlightingConsumer consumer)
        {
            var maxLength = Threshold;

            var statementCount = statement.CountChildren<IStatement>();
            if (statementCount > maxLength)
            {
                var highlighting = new Highlighting(Warnings.Warning_MethodTooLong);
                consumer.AddHighlighting(highlighting, statement.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.MethodTooLongMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.MethodTooLongEnabled); }
        }
    }
}