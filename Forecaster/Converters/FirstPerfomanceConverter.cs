using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace daisybrand.forecaster.Converters
{
    public class FirstPerfomanceConverter:IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var perf = (daisybrand.forecaster.Controlers.Collections.PerformanceCollection)value;
               
                 if (perf.FIRST == null) return null;
                return perf.FIRST_DISPLAY_OBJ ?? perf.FIRST_DISPLAY;
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
