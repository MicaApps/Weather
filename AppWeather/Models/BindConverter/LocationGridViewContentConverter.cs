using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppWeather.Models.BindConverter
{
    public class LocationGridViewContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)return Visibility.Collapsed;
            var v = String.IsNullOrEmpty((String)value);
            return v ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility == true)
                return DependencyProperty.UnsetValue;
            if ((Visibility)value == Visibility.Visible)
                return false;
            else
                return true;
        }
    }
}
