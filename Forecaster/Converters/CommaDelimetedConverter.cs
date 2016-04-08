using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
namespace daisybrand.forecaster.Converters
{
    public class CommaDelimetedConverter:IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int i;
            if (value != null && int.TryParse(value.ToString().Replace(",",""), out i))
            {
                var a = i.ToString("N0");
                return a;
            }
            return string.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
               return value;
        }

        #endregion

    }
}
