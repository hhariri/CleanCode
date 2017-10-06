using CleanCode;
using CleanCode.Features.HollowNames;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

[assembly: RegisterConfigurableSeverity(HollowTypeNameHighlighting.SeverityID, null, 
    CleanCodeHighlightingGroupIds.CleanCode, "Hollow type name", "This type has a name that doesn't express its intent.",
    Severity.SUGGESTION)]

namespace CleanCode.Features.HollowNames
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class HollowTypeNameHighlighting : IHighlighting
    {
        internal const string SeverityID = "HollowTypeName";
        private readonly DocumentRange _documentRange;

        public HollowTypeNameHighlighting(string toolTip, DocumentRange documentRange)
        {
            ToolTip = toolTip;
            _documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => _documentRange;

        public string ToolTip { get; }

        public string ErrorStripeToolTip => ToolTip;

        public bool IsValid() => true;
    }
}