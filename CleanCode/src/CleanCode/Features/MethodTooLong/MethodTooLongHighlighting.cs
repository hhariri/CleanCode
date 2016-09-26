using CleanCode;
using CleanCode.Features.MethodTooLong;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

[assembly: RegisterConfigurableSeverity(MethodTooLongHighlighting.SeverityID, null, 
    CleanCodeHighlightingGroupIds.CleanCode, "Method too long", "The method is bigger than it should be.",
    Severity.SUGGESTION, false)]

namespace CleanCode.Features.MethodTooLong
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class MethodTooLongHighlighting : IHighlighting
    {
        internal const string SeverityID = "MethodTooLong";
        private readonly DocumentRange documentRange;

        public MethodTooLongHighlighting(DocumentRange documentRange)
        {
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => documentRange;
        public string ToolTip => Warnings.MethodTooLong;
        public string ErrorStripeToolTip => ToolTip;
        public bool IsValid() => true;
    }
}