using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppWeather.Models.BindConverter
{
    public class LocationGridViewHolderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)return Visibility.Visible;
            var v = String.IsNullOrEmpty((String)value);
            return v ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility == true)
                return DependencyProperty.UnsetValue;
            if ((Visibility)value == Visibility.Visible)
                return true;
            else
                return false;
        }
    }
}
