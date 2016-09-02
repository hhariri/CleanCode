using System.Linq;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ComplexExpression
{
    public class ComplexExpressionCheck : MonoValueCheck<IExpression, int>
    {
        public ComplexExpressionCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override int Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ComplexExpressionMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ComplexExpressionEnabled); }
        }

        protected override void ExecuteCore(IExpression constructorDeclaration, IHighlightingConsumer consumer)
        {
            var maxExpressions = Value;
            var depth = constructorDeclaration.GetChildrenRecursive<IOperatorExpression>().Count();

            if (depth > maxExpressions)
            {
                var documentRange = constructorDeclaration.GetDocumentRange();
                var highlighting = new ExcessiveIndentation.Highlighting(Warnings.ExpressionTooComplex, documentRange);
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}