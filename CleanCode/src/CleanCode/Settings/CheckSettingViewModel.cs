using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using JetBrains.Application.Settings;

namespace CleanCode.Settings
{
    public class CheckSettingViewModel : ViewModel
    {
        private readonly Expression<Func<CleanCodeSettings, bool>> isEnabledSelector;
        
        protected readonly IContextBoundSettingsStore Settings;

        public CheckSettingViewModel([NotNull] IContextBoundSettingsStore settings,
            [NotNull] Expression<Func<CleanCodeSettings, bool>> isEnabledSelector)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (isEnabledSelector == null)
            {
                throw new ArgumentNullException("isEnabledSelector");
            }

            this.Settings = settings;
            this.isEnabledSelector = isEnabledSelector;            
        }

        public string Category { get; set; }

        public bool IsEnabled
        {
            get { return Settings.GetValue(isEnabledSelector); }
            set
            {
                Settings.SetValue(isEnabledSelector, value);
                OnPropertyChanged();
            }
        }

        public string IsEnabledDescription { get; set; }
    }
}