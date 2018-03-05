using CleanCode;
using CleanCode.Features.MethodTooLong;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

[assembly: RegisterConfigurableSeverity(MethodTooManyDeclarationsHighlighting.SeverityID, null,
    CleanCodeHighlightingGroupIds.CleanCode, "Too many Declarations", "The method has more declarations than there should be.",
    Severity.SUGGESTION)]

namespace CleanCode.Features.MethodTooLong
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class MethodTooManyDeclarationsHighlighting : IHighlighting
    {
        internal const string SeverityID = "TooManyDeclarations";
        private readonly DocumentRange _documentRange;

        public MethodTooManyDeclarationsHighlighting(DocumentRange documentRange, int threshold, int currentValue)
        {
            ToolTip = string.Format(Warnings.TooManyDeclarations, currentValue, threshold);
            _documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => _documentRange;

        public string ToolTip { get; } 

        public string ErrorStripeToolTip => ToolTip;

        public bool IsValid() => true;
    }
}