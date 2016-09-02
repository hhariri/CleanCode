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

    public class ComplexExpressionCheck : MonoValueCheck<IExpression, int>
    {
        public ComplexExpressionCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IExpression constructorDeclaration, IHighlightingConsumer consumer)
        {
            var maxExpressions = this.Value;
            var depth = constructorDeclaration.GetChildrenRecursive<IOperatorExpression>().Count();

            if (depth > maxExpressions)
            {
                var highlighting = new ExcessiveIndentation.Highlighting(Warnings.ExpressionTooComplex);
                consumer.AddHighlighting(highlighting, constructorDeclaration.GetDocumentRange());
            }
        }

        protected override int Value
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.ComplexExpressionMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.ComplexExpressionEnabled); }
        }
    }
}