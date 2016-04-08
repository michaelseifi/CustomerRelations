using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Interfaces;

namespace daisybrand.forecaster.Converters
{
    public class SkusConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                ICustomer data = (ICustomer)value;

                return data.SKUS;
            }
            catch(Exception ex)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
