using CleanCode;
using CleanCode.Features.ComplexExpression;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly:RegisterConfigurableSeverity(ComplexConditionExpressionHighlighting.SeverityID, null,
    CleanCodeHighlightingGroupIds.CleanCode, "Condition expression too complex", "The expression in the condition is too complex.",
    Severity.WARNING, false)]

namespace CleanCode.Features.ComplexExpression
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class ComplexConditionExpressionHighlighting : IHighlighting
    {
        internal const string SeverityID = "ComplexConditionExpression";

        private readonly DocumentRange documentRange;

        public ComplexConditionExpressionHighlighting(DocumentRange documentRange)
        {
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => documentRange;
        public string ToolTip => Warnings.ExpressionTooComplex;
        public string ErrorStripeToolTip => ToolTip;
        public bool IsValid() => true;
    }
}