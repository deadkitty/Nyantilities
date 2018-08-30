using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Nyantilities.Converter
{
    /// <summary>
    /// Converts boolean 'true' into Visibility 'Visible' and 'false' into 'Collapsed'
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts boolean 'true' into Visibility 'Collapsed' and 'false' into 'Visible'
    /// </summary>
    public class CollapsedConverter : VisibilityConverter
    {
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            return base.Convert(!(bool)value, targetType, parameter, language);
        }
    }
}
