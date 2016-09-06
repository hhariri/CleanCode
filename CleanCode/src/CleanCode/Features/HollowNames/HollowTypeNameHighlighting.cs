using CleanCode;
using CleanCode.Features.HollowNames;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(HollowTypeNameHighlighting.SeverityID, null, 
    CleanCodeHighlightingGroupIds.CleanCode, "Hollow type name", "This type has a name that doesn't express its intent.",
    Severity.SUGGESTION, false)]

namespace CleanCode.Features.HollowNames
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class HollowTypeNameHighlighting : IHighlighting
    {
        internal const string SeverityID = "HollowTypeName";
        private readonly string tooltip;
        private readonly DocumentRange documentRange;

        public HollowTypeNameHighlighting(string toolTip, DocumentRange documentRange)
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