using System;
using System.Linq.Expressions;
using JetBrains.Application.Settings;

namespace CleanCode.Settings
{
    public class CheckSettingViewModel : ViewModel
    {
        private readonly Expression<Func<CleanCodeSettings, bool>> isEnabledSelector;
        
        protected readonly IContextBoundSettingsStore Settings;

        public CheckSettingViewModel(IContextBoundSettingsStore settings, Expression<Func<CleanCodeSettings, bool>> isEnabledSelector)
        {
            this.Settings = settings;
            this.isEnabledSelector = isEnabledSelector;
            
        }

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