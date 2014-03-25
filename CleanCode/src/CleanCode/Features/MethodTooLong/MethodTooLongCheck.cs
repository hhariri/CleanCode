using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Features.Browsing.CallHierarchy.Strategy;
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

        protected override void ExecuteCore(IMethodDeclaration methodDeclaration, IHighlightingConsumer context)
        {
            var maxLength = Threshold;

            var statementCount = methodDeclaration.CountChildren<IStatement>();
            if (statementCount > maxLength)
            {
                var highlighting = new Highlighting(Common.Warning_MethodTooLong);
                context.AddHighlighting(highlighting, methodDeclaration.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumMethodLines); }
        }


        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.MethodTooLongEnabled); }
        }
    }
}