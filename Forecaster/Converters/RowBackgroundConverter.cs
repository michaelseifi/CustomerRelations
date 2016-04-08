using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using daisybrand.forecaster.Controlers.Collections;

namespace daisybrand.forecaster.Converters
{
    public class RowBackgroundConverter: MarkupExtension ,IValueConverter
    {
        SolidColorBrush TPRColor =  new SolidColorBrush(Colors.LightBlue);
        SolidColorBrush ADColor = new SolidColorBrush(Colors.LightGreen);

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    var col = (PerformanceCollection)value;
                    //PerformanceCollection col = data.PERFORMANCES;
                    if (col.ANY)
                    {
                        SolidColorBrush color;
                        switch (col.FIRST.PERFORMANCE_TYPE)
                        {
                            case "TPR":
                                color = TPRColor;
                                break;
                            case "AD":
                                color = ADColor;
                                break;
                            default:
                                color = new SolidColorBrush(Colors.LightCoral);
                                break;
                        }
                        return color;                      
                    }
                }
                catch { }
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
