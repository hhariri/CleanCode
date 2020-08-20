using System.Globalization;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

namespace CleanCode.Features.ChainedReferences
{
    [RegisterConfigurableSeverity(SeverityID,
        null,
        CleanCodeHighlightingGroupIds.CleanCode,
        "Too many chained references",
        "Too many chained references can break the Law of Demeter.",
        Severity.WARNING)]
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class MaximumChainedReferencesHighlighting : IHighlighting
    {
        internal const string SeverityID = "TooManyChainedReferences";

        private readonly DocumentRange _documentRange;

        public MaximumChainedReferencesHighlighting(DocumentRange documentRange, int threshold, int currentValue)
        {
            ToolTip = string.Format(CultureInfo.CurrentCulture,
                Warnings.ChainedReferences,
                currentValue,
                threshold);

            _documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => _documentRange;

        public string ToolTip { get; }

        public string ErrorStripeToolTip => ToolTip;

        public bool IsValid() => true;
    }
}