using CleanCode;
using CleanCode.Features.TooManyDependencies;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(TooManyDependenciesHighlighting.SeverityID, null,
    CleanCodeHighlightingGroupIds.CleanCode, "Too many dependencies", "Too many dependencies passed into constructor.",
    Severity.WARNING, false)]

namespace CleanCode.Features.TooManyDependencies
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class TooManyDependenciesHighlighting : IHighlighting
    {
        internal const string SeverityID = "TooManyDependencies";
        private readonly DocumentRange documentRange;

        public TooManyDependenciesHighlighting(DocumentRange documentRange)
        {
            this.documentRange = documentRange;
        }

        public DocumentRange CalculateRange()
        {
            return documentRange;
        }

        public string ToolTip
        {
            get { return Warnings.TooManyDependencies; }
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