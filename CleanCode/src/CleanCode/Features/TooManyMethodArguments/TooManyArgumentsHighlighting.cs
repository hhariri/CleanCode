using CleanCode;
using CleanCode.Features.TooManyMethodArguments;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

[assembly: RegisterConfigurableSeverity(TooManyArgumentsHighlighting.SeverityID, null, 
    CleanCodeHighlightingGroupIds.CleanCode, "Too many arguments", "Too many arguments passed to a method.",
    Severity.WARNING)]

namespace CleanCode.Features.TooManyMethodArguments
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class TooManyArgumentsHighlighting : IHighlighting
    {
        internal const string SeverityID = "TooManyArguments";
        private readonly DocumentRange _documentRange;

        public TooManyArgumentsHighlighting(string toolTip, DocumentRange documentRange)
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