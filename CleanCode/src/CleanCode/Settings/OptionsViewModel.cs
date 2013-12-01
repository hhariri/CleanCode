using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.UI.Application;
using JetBrains.UI.Options;
using IContextBoundSettingsStore = JetBrains.Application.Settings.IContextBoundSettingsStore;

namespace CleanCode.Settings
{

    public class OptionsViewModel : INotifyPropertyChanged
    {
        private Lifetime lifetime;
        private IUIApplication environment;
        private OptionsSettingsSmartContext settings;
        private int maxMethodArguments;
        private bool enableTooManyMethodArguments;
        private bool enableTooManyDependencies;
        private bool enabledMethodTooLong;
        private int maxDependencies;
        private int maxLinesPerMethod;
        private bool enabledMaxDepth;
        private int maxDepth;

        public OptionsViewModel(OptionsSettingsSmartContext settings)
        {
            this.settings = settings;


            EnabledMethodTooLong = settings.GetValue((CleanCodeSettings e) => e.MethodTooLongEnabled);
            EnableTooManyDependencies = settings.GetValue((CleanCodeSettings e) => e.MaximumDependenciesEnabled);
            EnableTooManyMethodArguments = settings.GetValue((CleanCodeSettings e) => e.MaximumMethodArgumentsEnabled);
            EnabledMaxDepth = settings.GetValue((CleanCodeSettings e) => e.MaximumCodeDepthEnabled);

            MaxMethodArguments = settings.GetValue((CleanCodeSettings e) => e.MaximumMethodArguments);
            MaxLinesPerMethod = settings.GetValue((CleanCodeSettings e) => e.MaximumMethodLines);
            MaxDependencies = settings.GetValue((CleanCodeSettings e) => e.MaximumDependencies);
            MaxDepth = settings.GetValue((CleanCodeSettings e) => e.MaximumCodeDepth);
        }

        public bool EnabledMethodTooLong
        {
            get { return enabledMethodTooLong; }
            set
            {
                if (value.Equals(enabledMethodTooLong)) return;
                enabledMethodTooLong = value;
                settings.SetValue((CleanCodeSettings e) => e.MethodTooLongEnabled, value);
                OnPropertyChanged();
            }
        }

        public bool EnableTooManyDependencies
        {
            get { return enableTooManyDependencies; }
            set
            {
                if (value.Equals(enableTooManyDependencies)) return;
                enableTooManyDependencies = value;
                settings.SetValue((CleanCodeSettings e) => e.MaximumDependenciesEnabled, value);
                OnPropertyChanged();
            }
        }

        public bool EnableTooManyMethodArguments
        {
            get { return enableTooManyMethodArguments; }
            set
            {
                if (value.Equals(enableTooManyMethodArguments)) return;
                enableTooManyMethodArguments = value;
                settings.SetValue((CleanCodeSettings e) => e.MaximumMethodArgumentsEnabled, value);
                OnPropertyChanged();
            }
        }

        public int MaxMethodArguments
        {
            get { return maxMethodArguments; }
            set
            {
                if (value == maxMethodArguments) return;
                maxMethodArguments = value;
                settings.SetValue((CleanCodeSettings e) => e.MaximumMethodArguments, value);
                OnPropertyChanged();                
            }
        }

        public int MaxDependencies
        {
            get { return maxDependencies; }
            set
            {
                if (value == maxDependencies) return;
                maxDependencies = value;
                settings.SetValue((CleanCodeSettings e) => e.MaximumDependencies, value);
                OnPropertyChanged();
            }
        }

        public int MaxLinesPerMethod
        {
            get { return maxLinesPerMethod; }
            set
            {
                if (value == maxLinesPerMethod) return;
                maxLinesPerMethod = value;
                settings.SetValue((CleanCodeSettings e) => e.MaximumMethodLines, value);
                OnPropertyChanged();
            }
        }

        public bool EnabledMaxDepth
        {
            get { return enabledMaxDepth; }
            set
            {
                if (Equals(value, enabledMaxDepth)) return;
                enabledMaxDepth = value;
                settings.SetValue((CleanCodeSettings e) => e.MaximumCodeDepthEnabled, value);
                OnPropertyChanged();
            }
        }

        public int MaxDepth
        {
            get { return maxDepth; }
            set
            {
                if (value == maxDepth) return;
                maxDepth = value;
                settings.SetValue((CleanCodeSettings e) => e.MaximumCodeDepth, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}