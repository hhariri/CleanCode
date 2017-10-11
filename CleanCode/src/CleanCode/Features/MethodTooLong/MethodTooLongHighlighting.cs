using System;
using CleanCode;
using CleanCode.Features.MethodTooLong;
using CleanCode.Resources;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.VB;

[assembly: RegisterConfigurableSeverity(MethodTooLongHighlighting.SeverityID, null, 
    CleanCodeHighlightingGroupIds.CleanCode, "Method too long", "The method is bigger than it should be.",
    Severity.SUGGESTION)]

namespace CleanCode.Features.MethodTooLong
{
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name + "," + VBLanguage.Name)]
    public class MethodTooLongHighlighting : IHighlighting
    {
        internal const string SeverityID = "MethodTooLong";
        private readonly DocumentRange _documentRange;

        public MethodTooLongHighlighting(DocumentRange documentRange, int threshold, int currentValue)
        {
            ToolTip = string.Format(Warnings.MethodTooLong, currentValue, threshold);
            _documentRange = documentRange;
        }

        public DocumentRange CalculateRange() => _documentRange;

        public string ToolTip { get; } 

        public string ErrorStripeToolTip => ToolTip;

        public bool IsValid() => true;
    }
}