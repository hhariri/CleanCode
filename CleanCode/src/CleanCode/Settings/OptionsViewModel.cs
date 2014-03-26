using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.Application.Settings.Logging;
using JetBrains.Application.Settings.Store.Implementation;
using JetBrains.UI.Options;
using IContextBoundSettingsStore = JetBrains.Application.Settings.IContextBoundSettingsStore;

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

        private static ArrayList GetViewModels(IContextBoundSettingsStore settings)
        {
            var viewModels = new ArrayList();

            var methodTooLong = new SingleCheckSettingViewModel<int>(
                settings, codeSettings => codeSettings.MethodTooLongEnabled,
                codeSettings => codeSettings.MaximumMethodLines);
            viewModels.Add(methodTooLong);

            //var tooManyDependencies = new SingleCheckSettingViewModel<int>(settings.GetValue(
            //    (CleanCodeSettings cleanCodeSettings) => cleanCodeSettings),
            //    e => e.MaximumDependenciesEnabled,
            //    e => e.MaximumDependencies);
            //viewModels.Add(tooManyDependencies);

            //var tooManyMethodArguments = new SingleCheckSettingViewModel<int>(settings.GetValue(
            //    (CleanCodeSettings cleanCodeSettings) => cleanCodeSettings),
            //    e => e.MaximumMethodArgumentsEnabled,
            //    e => e.MaximumMethodArguments);
            //viewModels.Add(tooManyMethodArguments);

            //var enabledMaxDepth = new SingleCheckSettingViewModel<int>(settings.GetValue(
            //    (CleanCodeSettings cleanCodeSettings) => cleanCodeSettings),
            //    e => e.MaximumDependenciesEnabled,
            //    e => e.MaximumCodeDepth);
            //viewModels.Add(enabledMaxDepth);

            //var enableClassTooBig = new SingleCheckSettingViewModel<int>(settings.GetValue(
            //    (CleanCodeSettings cleanCodeSettings) => cleanCodeSettings),
            //    e => e.MaximumMethodsPerClassEnabled,
            //    e => e.MaximumMethodsPerClass);
            //viewModels.Add(enableClassTooBig);

            //var enableChainedReferences = new SingleCheckSettingViewModel<int>(settings.GetValue(
            //    (CleanCodeSettings cleanCodeSettings) => cleanCodeSettings),
            //    e => e.MaximumChainedReferencesEnabled,
            //    e => e.MaximumChainedReferences);
            //viewModels.Add(enableChainedReferences);

            //var enableMinimumMethodNameLength = new SingleCheckSettingViewModel<int>(settings.GetValue(
            //    (CleanCodeSettings cleanCodeSettings) => cleanCodeSettings),
            //    e => e.MinimumMethodNameLenghtEnabled,
            //    e => e.MinimumMethodNameLenght);
            //viewModels.Add(enableMinimumMethodNameLength);

            return viewModels;
        }
    }
}