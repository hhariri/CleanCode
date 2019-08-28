using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace CleanCode.Settings
{
    [SettingsKey(typeof(CodeInspectionSettings), "CleanCode")]
    public class CleanCodeSettings
    {
        [SettingsEntry(3, nameof(MaximumConstructorDependencies))]
        public int MaximumConstructorDependencies { get; set; }

        [SettingsEntry(3, nameof(MaximumMethodParameters))]
        public int MaximumMethodParameters { get; set; }

        [SettingsEntry(15, nameof(MaximumMethodStatements))]
        public int MaximumMethodStatements { get; set; }

        [SettingsEntry(6, nameof(MaximumDeclarationsInMethod))]
        public int MaximumDeclarationsInMethod { get; set; }

        [SettingsEntry(3, nameof(MaximumIndentationDepth))]
        public int MaximumIndentationDepth { get; set; }

        [SettingsEntry(20, nameof(MaximumMethodsInClass))]
        public int MaximumMethodsInClass { get; set; }

        [SettingsEntry(2, nameof(MaximumChainedReferences))]
        public int MaximumChainedReferences { get; set; }

        [SettingsEntry(4, nameof(MinimumMeaningfulMethodNameLength))]
        public int MinimumMeaningfulMethodNameLength { get; set; }

        [SettingsEntry("Handler,Manager,Processor,Controller,Helper", nameof(MeaninglessClassNameSuffixes))]
        public string MeaninglessClassNameSuffixes { get; set; }

        [SettingsEntry(1, nameof(MaximumExpressionsInCondition))]
        public int MaximumExpressionsInCondition { get; set; }
    }
}