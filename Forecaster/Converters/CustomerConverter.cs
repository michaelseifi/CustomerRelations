using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using daisybrand.forecaster.Controlers.Collections;
namespace daisybrand.forecaster.Converters
{
    public class CustomerConverter:IValueConverter
    {
        #region IValueConverter Members
        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var coll = (CustomerCollection)value;
                return coll.Select(x=>x.ACCOUNT_NAME).OrderBy(x=>x).Distinct() ?? null;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
