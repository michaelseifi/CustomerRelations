using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Text;
namespace daisybrand.forecaster.Converters
{
    public class PerformanceMStateConverter
        : IValueConverter
    {

        #region IValueConverter Members
  
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return (Controlers.Enums.MState)(int.Parse(value.ToString()));
            }
            return Controlers.Enums.MState.Confirmed;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var mState = (int)Enum.Parse(typeof(Controlers.Enums.MState), value.ToString());
                return mState;
            }
            catch
            {
                return (int)Controlers.Enums.MState.Confirmed;
            }

        }

        #endregion
    }
}
