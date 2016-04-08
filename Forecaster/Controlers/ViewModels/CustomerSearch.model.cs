using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;

namespace daisybrand.forecaster.Controlers.ViewModels
{
    public class CustomerSearch:INotifyPropertyChanged
    {

        public CustomerSearch()
        {
            
        }
        #region properties
        private string _SELECTED_CUSTOMER_PREVIOUS;
        private string _SELECTED_USER_PREVIOUS;
        private string _SELECTED_SKU_PREVIOUS;
        private string _SELECTED_VIEW_PREVIOUS;
        private string _SELECTED_VIEW;
        private Sku _SELECTED_SKU;
        private IUser _SELECTED_USER;
        private ICustomer _SELECTED_CUSTOMER;
        
        private bool _IS_USER_BOX_SELECTED;
        private bool _IS_SKU_BOX_SELECTED;
        private bool _IS_CUSTOMER_BOX_SELECTED;       


        public ICustomer SELECTED_CUSTOMER
        {
            get
            {
                return _SELECTED_CUSTOMER;
            }
            set
            {
                _SELECTED_CUSTOMER = value;
                
                IS_CUSTOMER_BOX_SELECTED = value != null;
                
                if (ViewModels.MainWindow.GetTABS() == null && value != null)
                {
                    //RESET CUSTOMER COMMENT ICON
                    var set = value.SETTING;
                    daisybrand.forecaster.MainWindow.myMainWindowViewModel.SetIsThereCustomerComment(set);
                    //RESET CUSTOEMR STORE COUNT
                    //daisybrand.forecaster.MainWindow.myTopMenuViewModel.RefreshSTORE_COUNT();
                    //RESET ADD URL
                    //daisybrand.forecaster.MainWindow.myStatusBarViewModel.ADD_URL = value.SETTING.ADD_URL_URI;
                    
                }
                
                

                //if (daisybrand.forecaster.MainWindow.myUsers.FocusedEmplid != null)
                //    daisybrand.forecaster.MainWindow.myUsers.FocusedEmplid.CUSTOMERS.FocusedCustomer = (Customer)value;
                NotifyPropertyChanged();
            }
        }
        public string SELECTED_CUSTOMER_PREVIOUS
        {
            get
            {
                return _SELECTED_CUSTOMER_PREVIOUS;
            }
            set
            {
                _SELECTED_CUSTOMER_PREVIOUS = value;
                NotifyPropertyChanged();
            }
        }

        public IUser SELECTED_USER
        {
            get
            {
                return _SELECTED_USER;
            }
            set
            {
                _SELECTED_USER = value;
                IS_USER_BOX_SELECTED = value != null;
                daisybrand.forecaster.MainWindow.myUsers.FocusedEmplid = value;
                //if (SELECTED_CUSTOMER != null)
                //    daisybrand.forecaster.MainWindow.myUsers.FocusedEmplid.CUSTOMERS.FocusedCustomer = SELECTED_CUSTOMER as Customer;
                daisybrand.forecaster.MainWindow.myTopMenuViewModel.IS_USER_SELECTED = value != null;
                NotifyPropertyChanged();
            }
        }
        public string SELECTED_USER_PREVIOUS
        {
            get
            {
                return _SELECTED_USER_PREVIOUS;
            }
            set
            {
                _SELECTED_USER_PREVIOUS = value;
                NotifyPropertyChanged();
            }
        }

        public Sku SELECTED_SKU
        {
            get
            {
                return _SELECTED_SKU;
            }
            set
            {
                _SELECTED_SKU = value;

                IS_SKU_BOX_SELECTED = value != null;
                

                //if (daisybrand.forecaster.MainWindow.myUsers.FocusedEmplid.CUSTOMER_EXCEPTIONS.FocusedCustomer != null)
                //    daisybrand.forecaster.MainWindow.myUsers.FocusedEmplid.CUSTOMERS.FocusedCustomer.FocusedSku = value;

                NotifyPropertyChanged();
            }
        }

        public string SELECTED_SKU_PREVIOUS
        {
            get
            {
                return _SELECTED_SKU_PREVIOUS;
            }
            set
            {
                _SELECTED_SKU_PREVIOUS = value;
                NotifyPropertyChanged();
            }
        }

        public string SELECTED_VIEW
        {
            get
            {
                return _SELECTED_VIEW;
            }
            set
            {
                _SELECTED_VIEW = value;
                NotifyPropertyChanged();
            }
        }
        public string SELECTED_VIEW_PREVIOUS
        {
            get
            {
                return _SELECTED_VIEW_PREVIOUS;
            }
            set
            {
                _SELECTED_VIEW_PREVIOUS = value;
                NotifyPropertyChanged();
            }
        }
        public string ITERATION { get; set; }


        public bool IS_OK_BTN_ENABLED
        {
            get
            {
                var tabs = daisybrand.forecaster.MainWindow.myMainWindowViewModel.TABS;
                var tabIsLoading = tabs != null ? tabs.IsLoading : false;
                var result = IS_CUSTOMER_BOX_SELECTED && IS_SKU_BOX_SELECTED && IS_USER_BOX_SELECTED && !tabIsLoading && !daisybrand.forecaster.MainWindow.myMainWindowViewModel.IsActualOrderRereshing;
                daisybrand.forecaster.MainWindow.myTopMenuViewModel.IS_OK_BTN_ENABLED = result;
                return result;
            }            
        }

        public bool IS_CUSTOMER_BOX_SELECTED
        {
            get
            {
                return _IS_CUSTOMER_BOX_SELECTED;
            }
            set
            {
                _IS_CUSTOMER_BOX_SELECTED = value;
                NotifyPropertyChanged("IS_OK_BTN_ENABLED");
                NotifyPropertyChanged();
            }
        }
        public bool IS_SKU_BOX_SELECTED
        {
            get
            {
                return _IS_SKU_BOX_SELECTED;
            }
            set
            {
                _IS_SKU_BOX_SELECTED = value;
                NotifyPropertyChanged("IS_OK_BTN_ENABLED");
                NotifyPropertyChanged();
            }
        }
        public bool IS_USER_BOX_SELECTED
        {
            get
            {
                return _IS_USER_BOX_SELECTED;
            }
            set
            {
                _IS_USER_BOX_SELECTED = value;
                NotifyPropertyChanged("IS_OK_BTN_ENABLED");
                NotifyPropertyChanged();
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        

    }
}
