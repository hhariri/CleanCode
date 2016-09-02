using System;
using System.Linq.Expressions;
using JetBrains.Application.Settings;

namespace CleanCode.Settings
{
    public class MonoValueCheckSettingViewModel<TType> : CheckSettingViewModel
    {
        private readonly Expression<Func<CleanCodeSettings, TType>> valueSelector;

        public MonoValueCheckSettingViewModel(IContextBoundSettingsStore settings, Expression<Func<CleanCodeSettings, bool>> isEnabledSelector, Expression<Func<CleanCodeSettings, TType>> valueSelector) : base(settings, isEnabledSelector)
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