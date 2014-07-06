using System.Collections;
using JetBrains.Application.Settings;

namespace CleanCode.Settings
{
    public sealed class OptionsViewModel : ViewModel
    {
        public OptionsViewModel(IContextBoundSettingsStore settings)
        {
            Options = GetViewModels(settings);
        }

        public ArrayList Options { get; set; }

        // TODO: We need to refactor this method.
        // ReSharper disable once MethodTooLong
        private static ArrayList GetViewModels(IContextBoundSettingsStore settings)
        {
            var viewModels = new ArrayList();

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
                settings,
                codeSettings => codeSettings.MethodTooLongEnabled,
                codeSettings => codeSettings.MethodTooLongMaximum)
                           {
                               IsEnabledDescription = Resources.Settings.MethodTooLongCheck,
                               ValueDescription = Resources.Settings.TooLongLinesPerMethod,
                               Category = Categories.Size,
                           });

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
                settings,
                e => e.TooManyDependenciesMaximumEnabled,
                e => e.TooManyDependenciesMaximum)
                           {
                               IsEnabledDescription = Resources.Settings.MaximumDependenciesCheck,
                               ValueDescription = Resources.Settings.TooManyDependencies,
                               Category = Categories.Coupling,
                           });

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
                settings,
                e => e.TooManyMethodArgumentsEnabled,
                e => e.TooManyMethodArgumentsMaximum)
                           {
                               IsEnabledDescription = Resources.Settings.MaximumMethodArgumentsCheck,
                               ValueDescription = Resources.Settings.TooManyMethodArguments,
                               Category = Categories.Responsibility,
                           });

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
                settings,
                e => e.ExcessiveIndentationEnabled,
                e => e.ExcessiveIndentationMaximum)
                           {
                               IsEnabledDescription = Resources.Settings.ExcessiveDepthCheck,
                               ValueDescription = Resources.Settings.ExcessiveDepth,
                               Category = Categories.Coupling,
                           });

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
                settings,
                e => e.ClassTooBigEnabled,
                e => e.ClassTooBigMaximum)
                           {
                               IsEnabledDescription = Resources.Settings.ClassTooBigCheck,
                               ValueDescription = Resources.Settings.ClassTooBig,
                               Category = Categories.Size,
                           });

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
                settings,
                e => e.TooManyChainedReferencesEnabled,
                e => e.TooManyChainedReferencesMaximum)
                           {
                               IsEnabledDescription = Resources.Settings.MaxChainedReferencesCheck,
                               ValueDescription = Resources.Settings.MaxChainedReferences,
                               Category = Categories.Responsibility,
                           });

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
                settings,
                e => e.MethodNameNotMeaningfulMinimumEnabled,
                e => e.MethodNameNotMeaningfulMinimum)
                           {
                               IsEnabledDescription = Resources.Settings.MinimumMethodNameLengthCheck,
                               ValueDescription = Resources.Settings.MinimumMethodNameLength,
                               Category = Categories.Legibility,
                           });

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
                settings,
                e => e.ComplexExpressionEnabled,
                e => e.ComplexExpressionMaximum)
                           {
                               IsEnabledDescription = Resources.Settings.ComplexExpressionCheck,                
                               ValueDescription = Resources.Settings.ComplexExpressionMaximum,
                               Category = Categories.Complexity,
                           });

            viewModels.Add(new CheckSettingViewModel(
                settings,
                e => e.FlagArgumentsEnabled)
                           {
                               IsEnabledDescription = Resources.Settings.FlagArgumentsCheck,
                               Category = Categories.Responsibility,
                           });
           
            viewModels.Add(new MonoValueCheckSettingViewModel<string>(
                settings, 
                codeSettings => codeSettings.HollowTypeNameEnabled, 
                codeSettings => codeSettings.HollowTypeNameString)
                           {
                               IsEnabledDescription = Resources.Settings.HollowTypeNameCheck,
                               ValueDescription = Resources.Settings.HollowTypeNameWords,
                               Category = Categories.Legibility,
                           });

            viewModels.Add(new MonoValueCheckSettingViewModel<int>(
               settings,
               codeSettings => codeSettings.TooManyDeclarationsEnabled,
               codeSettings => codeSettings.TooManyDeclarationsMaximum)
            {
                IsEnabledDescription = Resources.Settings.TooManyDeclarationsCheck,
                ValueDescription = Resources.Settings.DeclarationsMaximum,
                Category = Categories.Responsibility,
            });

            return viewModels;
        }
    }

    public class Categories
    {
        public const string Size = "Size";
        public const string Complexity = "Complexity";
        public const string Coupling = "Coupling";
        public const string Responsibility = "Responsibility";
        public const string Legibility = "Legibility";
    }
}