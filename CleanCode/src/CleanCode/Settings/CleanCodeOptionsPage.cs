using CleanCode.Resources.Icons;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Daemon.OptionPages;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionsDialog2.SimpleOptions;

namespace CleanCode.Settings
{
    [OptionsPage(PageId, "Clean Code", typeof(SettingsThemedIcons.CleanCode), ParentId = CodeInspectionPage.PID)]
    public class CleanCodeOptionsPage : CustomSimpleOptionsPage
    {
        private const string PageId = "CleanCode";

        public CleanCodeOptionsPage(Lifetime lifetime, OptionsSettingsSmartContext optionsSettingsSmartContext)
            : base(lifetime, optionsSettingsSmartContext)
        {
            AddHeader("Single Responsibility");
            AddText("A class should only have a single responsibility. Do not do too much in a class or method.");
            AddIntOption((CleanCodeSettings s) => s.ClassTooBigMaximum,
                Resources.Settings.MaximumMethodsPerClass,
                "Too many method declarations in a class is an indicator that the class is doing too much.");
            AddIntOption((CleanCodeSettings s) => s.TooManyMethodArgumentsMaximum,
                Resources.Settings.MaximumMethodDeclarationParameters,
                "Too many parameters in a method declaration is an indicator of having more than one responsibility");
            AddIntOption((CleanCodeSettings s) => s.MethodTooLongMaximum, Resources.Settings.MaximumStatementsPerMethod,
                "Long methods are indicator of having more than one responsibility.");
            AddIntOption((CleanCodeSettings s) => s.TooManyDeclarationsMaximum,
                Resources.Settings.DeclarationsMaximum,
                "Too many variables are an indicator of having more than one responsibility.");
            AddIntOption((CleanCodeSettings s) => s.ExcessiveIndentationMaximum,
                Resources.Settings.MaximumLevelOfNestingInAMethod,
                "Too much nesting in a method is an indicator of having more than one responsibility.");

            AddHeader("Coupling");
            AddText("Avoid excessive coupling beween classes.");
            AddIntOption((CleanCodeSettings s) => s.TooManyDependenciesMaximum,
                Resources.Settings.MaximumConstructorDependencies);
            AddIntOption((CleanCodeSettings s) => s.TooManyChainedReferencesMaximum,
                Resources.Settings.MaximumChainedReferences,
                "Avoid breaking the Law of Demeter.");

            AddHeader("Legibility");
            AddText("Names should be meaningful.");
            AddIntOption((CleanCodeSettings s) => s.MethodNameNotMeaningfulMinimum,
                Resources.Settings.MinimumMethodNameLength);
            AddStringOption((CleanCodeSettings s) => s.HollowTypeNameString,
                Resources.Settings.MeaninglessNameSuffixes,
                Resources.Settings.MeaninglessNameSuffixesTooltip);

            AddHeader("Complexity");
            AddText("Reduce complexity in individual statements.");
            AddIntOption((CleanCodeSettings s) => s.ComplexExpressionMaximum,
                Resources.Settings.MaximumExpressionsInsideACondition);

            FinishPage();
        }
    }
}