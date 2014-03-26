using System;
using System.Linq.Expressions;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCleanup;
using JetBrains.UI.Options;

namespace CleanCode.Settings
{
    public class SingleCheckSettingViewModel<TType> : ViewModel
    {
        private readonly Expression<Func<CleanCodeSettings, bool>> getIsEnabled;
        private readonly Expression<Func<CleanCodeSettings, TType>> getValue;
        private readonly IContextBoundSettingsStore settings;

        public SingleCheckSettingViewModel(IContextBoundSettingsStore settings, Expression<Func<CleanCodeSettings, bool>> getIsEnabled, Expression<Func<CleanCodeSettings, TType>> getValue)
        {
            this.settings = settings;
            this.getIsEnabled = getIsEnabled;
            this.getValue = getValue;
        }

        public bool IsEnabled
        {
            get { return settings.GetValue(getIsEnabled); }
            set
            {
                settings.SetValue(getIsEnabled, value);
                OnPropertyChanged();
            }
        }

        public TType Value
        {
            get { return settings.GetValue(getValue); }
            set
            {
                settings.SetValue(getValue, value);
                OnPropertyChanged();
            }
        }

        public string IsEnabledDescription { get; set; }
        public string ValueDescription { get; set; }
    }
}