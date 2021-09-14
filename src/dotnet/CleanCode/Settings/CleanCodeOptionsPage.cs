using System;
using System.Linq.Expressions;
using CleanCode.Resources.Icons;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.IDE.UI.Extensions;
using JetBrains.IDE.UI.Options;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.Daemon.OptionPages;
using JetBrains.UI.RichText;

namespace CleanCode.Settings
{
    [OptionsPage(PageId, "Clean Code", typeof(SettingsThemedIcons.CleanCode), ParentId = CodeInspectionPage.PID)]
    public class CleanCodeOptionsPage : BeSimpleOptionsPage
    {
        private const string PageId = "CleanCodeAnalysisOptionsPage";

        public CleanCodeOptionsPage(
            Lifetime lifetime,
            OptionsPageContext optionsPageContext,
            OptionsSettingsSmartContext optionsSettingsSmartContext)
            : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
        {
            AddHeader("Single Responsibility");
            AddText("A class should only have a single responsibility. Do not do too much in a class or method.");

            AddIntOption((CleanCodeSettings s) => s.MaximumMethodsInClass, Resources.Settings.MaximumMethodsPerClass);
            AddIntOption((CleanCodeSettings s) => s.MaximumMethodParameters, Resources.Settings.MaximumMethodDeclarationParameters);
            AddIntOption((CleanCodeSettings s) => s.MaximumMethodStatements, Resources.Settings.MaximumStatementsPerMethod);
            AddIntOption((CleanCodeSettings s) => s.MaximumDeclarationsInMethod, Resources.Settings.DeclarationsMaximum);
            AddIntOption((CleanCodeSettings s) => s.MaximumIndentationDepth, Resources.Settings.MaximumLevelOfNestingInAMethod);

            AddHeader("Coupling");
            AddText("Avoid excessive coupling beween classes.");

            AddIntOption((CleanCodeSettings s) => s.MaximumConstructorDependencies, Resources.Settings.MaximumConstructorDependencies);
            AddIntOption((CleanCodeSettings s) => s.MaximumChainedReferences, Resources.Settings.MaximumChainedReferences);

            AddHeader("Legibility");
            AddText("Names should be meaningful.");

            AddIntOption((CleanCodeSettings s) => s.MinimumMeaningfulMethodNameLength, Resources.Settings.MinimumMethodNameLength);
            AddStringOption((CleanCodeSettings s) => s.MeaninglessClassNameSuffixes, Resources.Settings.MeaninglessNameSuffixes);

            AddHeader("Complexity");
            AddText("Reduce complexity in individual statements.");

            AddIntOption((CleanCodeSettings s) => s.MaximumExpressionsInCondition, Resources.Settings.MaximumExpressionsInsideACondition);

            AddSpacer();
            AddRichText(new RichText("Note: All references to Clean Code, including but not limited to the Clean Code icon are used with permission of Robert C. Martin (a.k.a. UncleBob)",
                new TextStyle(System.Drawing.FontStyle.Italic)));
        }

        private void AddIntOption<TKeyClass>(Expression<Func<TKeyClass, int>> expression, string description)
        {
            var valueProperty = OptionsSettingsSmartContext.GetValueProperty(Lifetime, expression);
            AddControl(valueProperty.GetBeSpinner(Lifetime, 0, 1000).WithDescription(description, Lifetime));
        }

        private void AddStringOption<TKeyClass>(Expression<Func<TKeyClass, string>> expression, string description)
        {
            var valueProperty = OptionsSettingsSmartContext.GetValueProperty(Lifetime, expression);
            AddControl(valueProperty.GetBeTextBox(Lifetime).WithDescription(description, Lifetime));
        }
    }
}