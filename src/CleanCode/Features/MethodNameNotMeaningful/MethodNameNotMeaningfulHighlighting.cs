using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

namespace CleanCode.Features.MethodNameNotMeaningful
{
    [RegisterConfigurableSeverity(SeverityID,
        null,
        CleanCodeHighlightingGroupIds.CleanCode,
        "Method name not meaningful",
        "This method name is too short to be meaningful.",
        Severity.WARNING)]
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class MethodNameNotMeaningfulHighlighting : IHighlighting
    {
        internal const string SeverityID = "MethodNameNotMeaningful";
        private readonly DocumentRange _documentRange;

        public MethodNameNotMeaningfulHighlighting(DocumentRange documentRange)
        {
            _documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => _documentRange;

        public string ToolTip => Warnings.MethodNameNotMeaningful;

        public string ErrorStripeToolTip => ToolTip;

        public bool IsValid() => true;
    }
}