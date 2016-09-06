using CleanCode.Features.FlagArguments;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(FlagArgumentsHighlighting.SeverityID, null, 
    HighlightingGroupIds.CodeSmell, "Flag argument", "An argument that is used as a flag.",
    Severity.WARNING, false)]

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

        public DocumentRange CalculateRange()
        {
            return documentRange;
        }

        public string ToolTip
        {
            get { return Warnings.FlagArgument; }
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