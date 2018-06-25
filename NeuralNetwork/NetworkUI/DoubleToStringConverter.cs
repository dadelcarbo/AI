using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp1
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double?) value)?.ToString(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => double.Parse(value.ToString(), culture);
    }
}
