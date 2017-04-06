using CleanCode;
using CleanCode.Features.ClassTooBig;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(ClassTooBigHighlighting.SeverityID, null,
    CleanCodeHighlightingGroupIds.CleanCode, "Class too big", "This class contains too many methods",
    Severity.SUGGESTION)]

namespace CleanCode.Features.ClassTooBig
{ 
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class ClassTooBigHighlighting : IHighlighting
    {
        internal const string SeverityID = "ClassTooBig";

        private readonly DocumentRange documentRange;

        public ClassTooBigHighlighting(string toolTip, DocumentRange documentRange)
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