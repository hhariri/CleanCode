using CleanCode.Features.MethodNameNotMeaningful;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(Highlighting.SeverityID, null, 
    HighlightingGroupIds.CodeSmell, "Method Name Not Meaningful",
    "This method name is to short to be meaningful. Please, choose a better name that describes its intent.",
    Severity.WARNING, false)]

namespace CleanCode.Features.MethodNameNotMeaningful
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class Highlighting : IHighlighting
    {
        internal const string SeverityID = "MethodNamesNotMeaningful";
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