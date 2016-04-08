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
    public class SkusItemNumberConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                var sku = (ISku)value;
                if (sku.SETTING != null && sku.SETTING.ITEM_NUM != null)
                    return sku.SETTING.ITEM_NUM.Trim();
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var focusedCustomer = MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER;
            focusedCustomer.FocusedSku.SETTING.ITEM_NUM = value.ToString();
            return value.ToString().ToString();
        }
    }
}
