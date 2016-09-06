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

        private readonly DocumentRange documentRange;

        public MaximumChainedReferencesHighlighting(string toolTip, DocumentRange documentRange)
        {
            ToolTip = toolTip;
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => documentRange;
        public string ToolTip { get; }
        public string ErrorStripeToolTip => ToolTip;
        public bool IsValid() => true;
    }
}