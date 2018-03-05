using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace CleanCode.Settings
{
    [SettingsKey(typeof(CodeInspectionSettings), "CleanCode")]
    public class CleanCodeSettings
    {
        [SettingsEntry(3, "MaximumConstructorDependencies")]
        public readonly int MaximumConstructorDependencies;

        [SettingsEntry(3, "MaximumMethodParameters")]
        public readonly int MaximumMethodParameters;

        [SettingsEntry(15, "MaximumMethodStatements")]
        public readonly int MaximumMethodStatements;

        [SettingsEntry(6, "MaximumDeclarationsInMethod")]
        public readonly int MaximumDeclarationsInMethod;

        [SettingsEntry(3, "MaximumIndentationDepth")]
        public int MaximumIndentationDepth { get; set; }

        [SettingsEntry(20, "MaximumMethodsInClass")]
        public int MaximumMethodsInClass { get; set; }

        [SettingsEntry(2, "MaximumChainedReferences")]
        public int MaximumChainedReferences { get; set; }

        [SettingsEntry(4, "MinimumMeaningfulMethodNameLength")]
        public int MinimumMeaningfulMethodNameLength { get; set; }

        [SettingsEntry("Handler,Manager,Processor,Controller,Helper", "MeaninglessClassNameSuffixes")]
        public string MeaninglessClassNameSuffixes { get; set; }  

        [SettingsEntry(1, "MaximumExpressionsInCondition")]
        public int MaximumExpressionsInCondition { get; set; }
    }
}