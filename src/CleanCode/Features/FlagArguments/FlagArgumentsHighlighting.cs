using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

namespace CleanCode.Features.FlagArguments
{
    [RegisterConfigurableSeverity(SeverityID,
        null,
        CleanCodeHighlightingGroupIds.CleanCode,
        "Flag argument",
        "An argument that is used as a flag.",
        Severity.WARNING)]
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class FlagArgumentsHighlighting : IHighlighting
    {
        internal const string SeverityID = "FlagArgument";
        private readonly DocumentRange _documentRange;

        public FlagArgumentsHighlighting(DocumentRange documentRange)
        {
            _documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => _documentRange;

        public string ToolTip => Warnings.FlagArgument;

        public string ErrorStripeToolTip => ToolTip;

        public bool IsValid() => true;
    }
}