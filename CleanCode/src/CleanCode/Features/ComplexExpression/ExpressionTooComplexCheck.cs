namespace CleanCode.Features.ComplexExpression
{
    using System.Linq;

    using CleanCode.Resources;
    using CleanCode.Settings;

    using JetBrains.Application.Settings;
    using JetBrains.ReSharper.Daemon.CSharp.Stages;
    using JetBrains.ReSharper.Daemon.Stages;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Tree;

    public class ComplexExpressionCheck : SimpleCheck<IExpression, int>
    {
        public ComplexExpressionCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IExpression typeExpression, IHighlightingConsumer consumer)
        {
            var maxExpressions = this.Threshold;
            var depth = typeExpression.GetChildrenRecursive<IOperatorExpression>().Count();

            if (depth > maxExpressions)
            {
                var highlighting = new ExcessiveIndentation.Highlighting(Warnings.ExpressionTooComplex);
                consumer.AddHighlighting(highlighting, typeExpression.GetDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.ComplexExpressionMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.ComplexExpressionEnabled); }
        }
    }
}