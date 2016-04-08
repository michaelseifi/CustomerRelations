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
    public class SkusCommentConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                ISku data = (ISku)value;
                if (data.SETTING != null)
                    return data.SETTING.COMMENT;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tab = MainWindow.myMainWindowViewModel.TABS.FOCUSEDTAB; ;
            return tab.SKU;
        }
    }
}
