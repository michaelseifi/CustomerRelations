using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Interfaces;

namespace daisybrand.forecaster.Converters
{
    public class FocusedDataConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDailyData data = (IDailyData)value;

            return data.DAILY_DATA;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
