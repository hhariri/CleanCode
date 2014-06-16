using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using JetBrains.Application.Settings;

namespace CleanCode.Settings
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

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

            var methodTooLong = new SingleCheckSettingViewModel<int>(
                settings,
                codeSettings => codeSettings.MethodTooLongEnabled,
                codeSettings => codeSettings.MethodTooLongMaximum)
                {
                    IsEnabledDescription = Resources.Settings.MethodTooLongCheck,
                    ValueDescription = Resources.Settings.TooLongLinesPerMethod,
                };
            viewModels.Add(methodTooLong);

            var tooManyDependencies = new SingleCheckSettingViewModel<int>(
                settings,
                e => e.TooManyDependenciesMaximumEnabled,
                e => e.TooManyDependenciesMaximum)
                {
                    IsEnabledDescription = Resources.Settings.MaximumDependenciesCheck,
                    ValueDescription = Resources.Settings.TooManyDependencies,
                };
            viewModels.Add(tooManyDependencies);

            var tooManyMethodArguments = new SingleCheckSettingViewModel<int>(
                settings,
                e => e.TooManyMethodArgumentsEnabled,
                e => e.TooManyMethodArgumentsMaximum)
                {
                    IsEnabledDescription = Resources.Settings.MaximumMethodArgumentsCheck,
                    ValueDescription = Resources.Settings.TooManyMethodArguments,
                };

            viewModels.Add(tooManyMethodArguments);

            var enabledMaxDepth = new SingleCheckSettingViewModel<int>(
                settings,
                e => e.TooManyDependenciesMaximumEnabled,
                e => e.ExcessiveIndentationMaximum)
                {
                    IsEnabledDescription = Resources.Settings.ExcessiveDepthCheck,
                    ValueDescription = Resources.Settings.ExcessiveDepth,
                };

            viewModels.Add(enabledMaxDepth);

            var enableClassTooBig = new SingleCheckSettingViewModel<int>(
                settings,
                e => e.ClassTooBigEnabled,
                e => e.ClassTooBigMaximum)
                {
                    IsEnabledDescription = Resources.Settings.ClassTooBigCheck,
                    ValueDescription = Resources.Settings.ClassTooBig,
                };

            viewModels.Add(enableClassTooBig);

            var enableChainedReferences = new SingleCheckSettingViewModel<int>(
                settings,
                e => e.TooManyChainedReferencesEnabled,
                e => e.TooManyChainedReferencesMaximum)
                {
                    IsEnabledDescription = Resources.Settings.MaxChainedReferencesCheck,
                    ValueDescription = Resources.Settings.MaxChainedReferences,
                };

            viewModels.Add(enableChainedReferences);

            var enableMinimumMethodNameLength = new SingleCheckSettingViewModel<int>(
                settings,
                e => e.MethodNameNotMeaningfulMinimumEnabled,
                e => e.MethodNameNotMeaningfulMinimum)
                {
                    IsEnabledDescription = Resources.Settings.MinimumMethodNameLengthCheck,
                    ValueDescription = Resources.Settings.MinimumMethodNameLength,
                };

            viewModels.Add(enableMinimumMethodNameLength);

            return viewModels;
        }
    }
}