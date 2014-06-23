using System;
using System.Linq.Expressions;
using JetBrains.Application.Settings;

namespace CleanCode.Settings
{
    public class ThresholdCheckSettingViewModel<TType> : CheckSettingViewModel
    {
        protected Expression<Func<CleanCodeSettings, TType>> valueSelector;

        public ThresholdCheckSettingViewModel(IContextBoundSettingsStore settings, Expression<Func<CleanCodeSettings, bool>> isEnabledSelector, Expression<Func<CleanCodeSettings, TType>> valueSelector) : base(settings, isEnabledSelector)
        {
            this.valueSelector = valueSelector;
        }

        public TType Value
        {
            get { return Settings.GetValue(valueSelector); }
            set
            {
                Settings.SetValue(valueSelector, value);
                OnPropertyChanged();
            }
        }

        public string ValueDescription { get; set; }
    }
}