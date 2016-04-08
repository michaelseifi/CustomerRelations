using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Text;
namespace daisybrand.forecaster.Converters
{
    public class PerformanceDStateConverter
        : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var dState =  (int)(Enum.Parse(typeof(Controlers.Enums.DState), value.ToString()));
                return dState;
            }
            return (int)Controlers.Enums.DState.None;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var dState = Enum.Parse(typeof(Controlers.Enums.DState), value.ToString());
                return dState;
            }
            catch
            {
                return Controlers.Enums.DState.None;
            }

        }

        #endregion
    }
}
