using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Text;
namespace daisybrand.forecaster.Converters
{
    public class CellBackgroundConverter:IValueConverter
    {

        #region IValueConverter Members
        public RadialGradientBrush color { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ( value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                if (parameter != null && parameter.GetType().Equals(typeof(SolidColorBrush)))
                    return parameter;
                else
                    return color; // new SolidColorBrush(Colors.LightCoral);                
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
