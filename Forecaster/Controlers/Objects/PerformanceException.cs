using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class PerformanceException : IPerformanceException, INotifyPropertyChanged
    {
        #region properties
        private string _DESCRIPTION;
        private double _PRICE;
        private int _QUANTITY;
        private System.DateTime _TO;
        private System.DateTime _FROM;
        private string _TYPE;
        private int _CUSTOMER_ID;
        private System.Guid _GUID;
        public System.Guid GUID
        {
            get
            {
                return _GUID;
            }
            set
            {
                _GUID = value;
            }
        }
        public int CUSTOMER_ID
        {
            get
            {
                return _CUSTOMER_ID;
            }
            set
            {
                _CUSTOMER_ID = value;
            }
        }
        public string TYPE
        {
            get
            {
                return _TYPE;
            }
            set
            {
                _TYPE = value;
                NotifyPropertyChanged();
            }
        }
        public System.DateTime FROM
        {
            get
            {
                return _FROM;
            }
            set
            {
                _FROM = value;
                NotifyPropertyChanged();
            }
        }
        public System.DateTime TO
        {
            get
            {
                return _TO;
            }
            set
            {
                _TO = value;
                NotifyPropertyChanged();
            }
        }
        public int QUANTITY
        {
            get
            {
                return _QUANTITY;
            }
            set
            {
                _QUANTITY = value;
                NotifyPropertyChanged();
            }
        }
        public double PRICE
        {
            get
            {
                return _PRICE;
            }
            set
            {
                _PRICE = value;
                NotifyPropertyChanged();
            }
        }
        public string DESCRIPTION
        {
            get
            {
                return _DESCRIPTION;
            }
            set
            {
                _DESCRIPTION = value;
                NotifyPropertyChanged();
            }
        }
        public List<string> TypeArray
        {
            get
            {
                var list = new List<string>();
                list.Add("TPR");
                list.Add("AD");
                return list;
            }
        }

        #endregion
   


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
