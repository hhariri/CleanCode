using CleanCode.Features.ClassTooBig;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(Highlighting.SeverityID, null,
    HighlightingGroupIds.CodeSmell, "Class Too Big", "This class contains too many methods",
    Severity.SUGGESTION, false)]

namespace CleanCode.Features.ClassTooBig
{ 
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class Highlighting : IHighlighting
    {
        internal const string SeverityID = "ClassTooBig";
        private readonly string tooltip;
        private readonly DocumentRange documentRange;

        public Highlighting(string toolTip, DocumentRange documentRange)
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