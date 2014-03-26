using System;
using System.Linq.Expressions;
using JetBrains.Application.Settings;

namespace CleanCode.Settings
{
    public class SingleCheckSettingViewModel<TType> : ViewModel
    {
        private readonly Expression<Func<CleanCodeSettings, bool>> isEnabledSelector;
        private readonly Expression<Func<CleanCodeSettings, TType>> valueSelector;
        private readonly IContextBoundSettingsStore settings;

        public SingleCheckSettingViewModel(IContextBoundSettingsStore settings, Expression<Func<CleanCodeSettings, bool>> isEnabledSelector, Expression<Func<CleanCodeSettings, TType>> valueSelector)
        {
            this.settings = settings;
            this.isEnabledSelector = isEnabledSelector;
            this.valueSelector = valueSelector;
        }

        public bool IsEnabled
        {
            get { return settings.GetValue(isEnabledSelector); }
            set
            {
                settings.SetValue(isEnabledSelector, value);
                OnPropertyChanged();
            }
        }

        public TType Value
        {
            get { return settings.GetValue(valueSelector); }
            set
            {
                settings.SetValue(valueSelector, value);
                OnPropertyChanged();
            }
        }

        public string IsEnabledDescription { get; set; }
        public string ValueDescription { get; set; }
    }
}