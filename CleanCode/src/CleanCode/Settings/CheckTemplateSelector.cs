using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CleanCode.Settings
{
    public class CheckTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ThresholdCheckTemplate { get; set; }
        public DataTemplate BasicCheckTemplate { get; set; }
        public DataTemplate StringCheckTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return base.SelectTemplate(item, container);
            }

            var type = item.GetType();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(MonoValueCheckSettingViewModel<>))
            {
                return GetTemplateFromGenericType(type);
            }
            if (item is CheckSettingViewModel)
            {
                return BasicCheckTemplate;
            }
            return base.SelectTemplate(item, container);
        }

        private DataTemplate GetTemplateFromGenericType(Type type)
        {
            var firstType = type.GetGenericArguments().First();
            if (firstType == typeof (string))
            {
                return StringCheckTemplate;
            }
            return ThresholdCheckTemplate;
        }


        bool IsTypeof<T>(object t)
        {
            return (t is T);
        }
    }
}