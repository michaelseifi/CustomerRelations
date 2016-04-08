using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
namespace daisybrand.forecaster.Converters
{
    public class MultiVisibilityConverter:IMultiValueConverter
    {

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null)
            {
                IEnumerable<bool> a = values.Cast<bool>();
                if (a.Any(x => x == true))
                    return Visibility.Visible;
            }
            if (parameter != null && parameter.ToString().ToUpper() == "collapse".ToUpper())
                return Visibility.Collapsed;
            return Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
