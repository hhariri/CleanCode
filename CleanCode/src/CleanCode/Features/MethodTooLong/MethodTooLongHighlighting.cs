using CleanCode;
using CleanCode.Features.MethodTooLong;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(MethodTooLongHighlighting.SeverityID, null, 
    CleanCodeHighlightingGroupIds.CleanCode, "Method too long", "The method is bigger than it should be.",
    Severity.SUGGESTION, false)]

namespace CleanCode.Features.MethodTooLong
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class MethodTooLongHighlighting : IHighlighting
    {
        internal const string SeverityID = "MethodTooLong";
        private readonly DocumentRange documentRange;

        public MethodTooLongHighlighting(DocumentRange documentRange)
        {
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange()
        {
            return documentRange;
        }

        public string ToolTip
        {
            get { return Warnings.MethodTooLong; }
        }

        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}