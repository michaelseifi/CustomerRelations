using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Collections;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class User : IUser, INotifyPropertyChanged
    {
        public User(string emplid, string status)
        {
            this.EMPLID = emplid;
            this.SELECTED_VALUE = emplid;
            this.DISPLAY_VALUE = emplid;
            this.CUSTOMERS = new CustomerCollection();
            this.SKUS = new SkuCollection();
            this.CUSTOMER_EXCEPTIONS = new CustomerCollection();
            STATUS = status;
        }
        
        
        private CustomerCollection _CUSTOMER_EXCEPTIONS;
        private SkuCollection _SKUS;
        private string _STATUS;
        private string _DISPLAY_VALUE;
        private CustomerCollection _CUSTOMERS;
        private string _EMPLID;
        private string _SELECTED_VALUE;
                
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
        
        #region IUser Members


        public string STATUS
        {
            get
            {
                return _STATUS;
            }
            set
            {
                if (_STATUS == value) return;
                _STATUS = value;
                NotifyPropertyChanged();
            }
        }

        public string DISPLAY_VALUE
        {
            get
            {
                return _DISPLAY_VALUE;
            }
            set
            {
                _DISPLAY_VALUE = value;
                NotifyPropertyChanged();
            }
        }
        public string SELECTED_VALUE
        {
            get
            {
                return _SELECTED_VALUE;
            }
            set
            {
                _SELECTED_VALUE = value;
                NotifyPropertyChanged();
            }
        }

        public string EMPLID
        {
            get
            {
                return _EMPLID;
            }
            set
            {
                if (_EMPLID == value) return;
                _EMPLID = value;
                NotifyPropertyChanged();
            }
        }
        public CustomerCollection CUSTOMERS
        {
            get
            {
                return _CUSTOMERS;
            }
            set
            {
                _CUSTOMERS = value;
                NotifyPropertyChanged();
            }
        }

        public SkuCollection SKUS
        {
            get
            {
                return _SKUS;
            }
            set
            {
                _SKUS = value;
                NotifyPropertyChanged();
            }
        }

        public CustomerCollection CUSTOMER_EXCEPTIONS
        {
            get
            {
                return _CUSTOMER_EXCEPTIONS;
            }
            set
            {
                _CUSTOMER_EXCEPTIONS = value;
                NotifyPropertyChanged();
            }
        }
        #endregion


    }
}
