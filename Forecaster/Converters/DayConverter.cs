using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
namespace daisybrand.forecaster.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DayConverter : IValueConverter
    {        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime original = (DateTime)value;
                if (original > DateTime.MinValue)
                    return original.DayOfWeek.ToString();
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
