using CleanCode;
using CleanCode.Features.FlagArguments;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(FlagArgumentsHighlighting.SeverityID, null, 
    CleanCodeHighlightingGroupIds.CleanCode, "Flag argument", "An argument that is used as a flag.",
    Severity.WARNING)]

namespace CleanCode.Features.FlagArguments
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class FlagArgumentsHighlighting : IHighlighting
    {
        internal const string SeverityID = "FlagArgument";
        private readonly DocumentRange documentRange;

        public FlagArgumentsHighlighting(DocumentRange documentRange)
        {
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => documentRange;
        public string ToolTip => Warnings.FlagArgument;
        public string ErrorStripeToolTip => ToolTip;
        public bool IsValid() => true;
    }
}