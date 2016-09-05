using CleanCode;
using CleanCode.Features.ChainedReferences;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(MaximumChainedReferencesHighlighting.SeverityID, null,
    CleanCodeHighlightingGroupIds.CleanCode, "Too many chained references", "Too many chained references can break the Law of Demeter.",
    Severity.WARNING, false)]

namespace CleanCode.Features.ChainedReferences
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class MaximumChainedReferencesHighlighting : IHighlighting
    {
        internal const string SeverityID = "TooManyChainedReferences";
        private readonly string tooltip;
        private readonly DocumentRange documentRange;

        public MaximumChainedReferencesHighlighting(string toolTip, DocumentRange documentRange)
        {
            tooltip = toolTip;
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange()
        {
            return documentRange;
        }

        public string ToolTip
        {
            get { return tooltip; }
        }

        public string ErrorStripeToolTip
        {
            get { return tooltip; }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}