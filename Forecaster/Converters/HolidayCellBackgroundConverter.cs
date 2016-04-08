using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Interfaces;
using System.Windows.Media;

namespace daisybrand.forecaster.Converters
{
    public class HolidayCellBackgroundConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    
                    if ((bool)value)
                        return new SolidColorBrush(Colors.LightCoral);
                }
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
