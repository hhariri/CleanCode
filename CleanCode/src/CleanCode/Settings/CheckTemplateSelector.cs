using System.Windows;
using System.Windows.Controls;

namespace CleanCode.Settings
{
    public class CheckTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ThresholdCheckTemplate { get; set; }
        public DataTemplate BasicCheckTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var type = item.GetType();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ThresholdCheckSettingViewModel<>))
            {
                return ThresholdCheckTemplate;
            }
            if (item is CheckSettingViewModel)
            {
                return BasicCheckTemplate;
            }
            return base.SelectTemplate(item, container);
        }

        bool IsTypeof<T>(object t)
        {
            return (t is T);
        }
    }
}