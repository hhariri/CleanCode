using CleanCode.Features.MethodNameNotMeaningful;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(MethodNameNotMeaningfulHighlighting.SeverityID, null, 
    HighlightingGroupIds.CodeSmell, "Method name not meaningful",
    "This method name is too short to be meaningful.",
    Severity.WARNING, false)]

namespace CleanCode.Features.MethodNameNotMeaningful
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class MethodNameNotMeaningfulHighlighting : IHighlighting
    {
        internal const string SeverityID = "MethodNameNotMeaningful";
        private readonly DocumentRange documentRange;

        public MethodNameNotMeaningfulHighlighting(DocumentRange documentRange)
        {
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange()
        {
            return documentRange;
        }

        public string ToolTip
        {
            get { return Warnings.MethodNameNotMeaningful; }
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