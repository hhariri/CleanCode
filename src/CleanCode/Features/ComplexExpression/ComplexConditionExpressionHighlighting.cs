using System.Globalization;
using CleanCode;
using CleanCode.Features.ComplexExpression;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

[assembly: RegisterConfigurableSeverity(ComplexConditionExpressionHighlighting.SeverityID, null,
    CleanCodeHighlightingGroupIds.CleanCode, "Condition expression too complex", "The expression in the condition is too complex.",
    Severity.WARNING)]

namespace CleanCode.Features.ComplexExpression
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class ComplexConditionExpressionHighlighting : IHighlighting
    {
        internal const string SeverityID = "ComplexConditionExpression";

        private readonly DocumentRange _documentRange;

        public ComplexConditionExpressionHighlighting(DocumentRange documentRange, int threshold, int currentValue)
        {
            ToolTip = string.Format(CultureInfo.CurrentCulture, Warnings.ExpressionTooComplex, currentValue, threshold);
            _documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => _documentRange;

        public string ToolTip { get; }

        public string ErrorStripeToolTip => ToolTip;

        public bool IsValid() => true;
    }
}