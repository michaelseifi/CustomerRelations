using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Extensions;
namespace daisybrand.forecaster.Controlers.Fields
{
    public class BaseIndex : IField<string>, INotifyPropertyChanged
    {
        public BaseIndex(IDailyData data)
        {
            WEEK_ID = data.WEEK_ID;
            DATA = data;
        }


        #region IField<decimal> Members
        
        
        private string _VALUE;
        private string _WEEK_ID;
        public string WEEK_ID
        {
            get
            {
                return _WEEK_ID;
            }
            set
            {
                _WEEK_ID = value;
            }
        }

        public string VALUE
        {
            get
            {
                if (this.DATA.BASE_LINE.VALUE == 0) return string.Empty;
                var value = (this.DATA.QS.ToDecimal() / this.DATA.BASE_LINE.VALUE.ToDecimal()).ToString("0.##");
                return value;
            }
            set
            {
                _VALUE = value;
                NotifyPropertyChanged();
            }
        }

        public Objects.Comment COMMENT
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string TOOLTIP
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region properties

        public IDailyData DATA
        {
            get;
            set;
        }

        public decimal ToDecimal()
        {
            decimal result;
            if (Decimal.TryParse(VALUE, out result))
                return result;
            return 0;

        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
