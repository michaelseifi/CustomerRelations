using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace daisybrand.forecaster.Converters
{
    class IsValueLessThanANumberConverter : IValueConverter
    {
        #region IValueConverter Members


        public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal v;
            decimal p;
            if (value != null && decimal.TryParse(value.ToString(), out v))
            {
                if (decimal.TryParse(parameter.ToString(), out p))
                    return v <= p;                
                return v <= 0;
            }            
            return false;
        }

        public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
