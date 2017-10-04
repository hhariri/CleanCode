using CleanCode;
using CleanCode.Features.MethodNameNotMeaningful;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

[assembly: RegisterConfigurableSeverity(MethodNameNotMeaningfulHighlighting.SeverityID, null, 
    CleanCodeHighlightingGroupIds.CleanCode, "Method name not meaningful",
    "This method name is too short to be meaningful.",
    Severity.WARNING)]

namespace CleanCode.Features.MethodNameNotMeaningful
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class MethodNameNotMeaningfulHighlighting : IHighlighting
    {
        internal const string SeverityID = "MethodNameNotMeaningful";
        private readonly DocumentRange documentRange;

        public MethodNameNotMeaningfulHighlighting(DocumentRange documentRange)
        {
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => documentRange;
        public string ToolTip => Warnings.MethodNameNotMeaningful;
        public string ErrorStripeToolTip => ToolTip;
        public bool IsValid() => true;
    }
}