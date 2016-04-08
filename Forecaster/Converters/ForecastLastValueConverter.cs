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
    public class ForecastLastValueConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            int i;
            if (value != null)
            {
                ForecastCollection data = (ForecastCollection)value;
                if (int.TryParse(data.LASTVALUE.ToString(), out i))
                {
                    var a = i.ToString("N0");
                    return a;
                }
            }
            return string.Empty;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                //parse week id
                var s = value.ToString();
                var index = s.IndexOf(":");
                if (index > 0)
                {
                    var weekId = s.Substring(0, s.IndexOf(":"));
                    var lastValue = s.Substring(s.IndexOf(":") + 1);
                    var data = 
                        MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB.myWeeklyData
                        .Where(x => x.WEEK_ID == weekId).FirstOrDefault().FORECASTS;
                    data.LASTVALUE = (string)value;
                    return data;
                }
                else
                {
                    var tab = MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB;
                    var data = tab.myWeeklyData;
                    var forecasts = data.FocusedData.FORECASTS;
                    forecasts.LASTVALUE = (string)value;
                    data.GetDaysOfSuppliesAsync();
                    return data;
                }
            }
            return null;
        }
    }
}
