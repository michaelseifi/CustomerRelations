using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace daisybrand.forecaster.Controlers.Collections
{
    public class TabCollection : ObservableCollection<Tab>, INotifyPropertyChanged, IDisposable
    {
        public TabCollection()
        {
            IsLoading = true;
        }
        public TabCollection(IEnumerable<ISku> skus, Interfaces.ICustomer customer)
        {
            IsLoading = true;
            FOCUSEDCUSTOMER = customer;
            foreach (var sku in skus)
                this.Add(new Tab
                {
                    CAPTION = sku.SKUID,
                    CUSTOMER_NUMBER = customer.ACCOUNTNUM,
                    SKU = sku
                });
        }
        public TabCollection(List<Tab> list)
            : base(list)
        {
            
        }
        public TabCollection(IEnumerable<Tab> collection)
            : base(collection)
        {

        }

        public Tab AddItem(Tab tab)
        {
            this.Add(tab);
            return tab;
        }

        public Tab GetTab(string skuId)
        {
            return this.First(t => t.SKU.SKUID == skuId.ToUpper());
        }

        #region properties
        private Interfaces.ICustomer _FOCUSEDCUSTOMER;
        private bool _IsLoading;
        private MainWindow _OWNER;
        private bool _IsLoaded;
        private Tab _FOCUSEDTAB;
        public Tab FOCUSEDTAB
        {
            get
            {
                return _FOCUSEDTAB;
            }
            set
            {
                _FOCUSEDTAB = value;
                //if (MainWindow.myTopMenuViewModel != null) MainWindow.myTopMenuViewModel.FOCUSED_TAB = value;
                if (value != null)
                {
                    FOCUSEDCUSTOMER.FocusedSku = value.SKU;
                    
                    MainWindow.myTopMenuViewModel.RefreshSkuSettings();
                    //if (value.myWeeklyData != null)
                    //    MainWindow.myTopMenuViewModel.RefreshAVG_TURNS();
                    
                    
                }
                NotifyPropertyChanged();
                MainWindow.myTopMenuViewModel.NotifyPropertyChanged("IsSkuFocused");
                MainWindow.myTopMenuViewModel.NotifyPropertyChanged("DAYS_OF_SUPPLIES");
            }
        }

        public Interfaces.ICustomer FOCUSEDCUSTOMER
        {
            get
            {
                return _FOCUSEDCUSTOMER;
            }
            set
            {
                _FOCUSEDCUSTOMER = value;      
                NotifyPropertyChanged();
            }
        }



        public bool IsLoading
        {
            get
            {
                return _IsLoading;
            }
            set
            {
                if (value == _IsLoading) return;
                _IsLoading = value;
                if (!value)
                {
                    OWNER.SetBusyIndicatorState(false);
                    MainWindow.myMainWindowViewModel.SetBUSYINDICATOR_TEXT_TimerAsync(false);
                    if (MinimizeWin) OWNER.NormalizeScreen();
                }
                MainWindow.myCustomerSearchViewModel.NotifyPropertyChanged("IS_OK_BTN_ENABLED");
                NotifyPropertyChanged();
            }
        }

        public bool CheckLoading()
        {
            if (!this.Any(t => t.IsLoaded == false))
                IsLoading = false;
            if (!IsLoading)
                MainWindow.myTopMenuViewModel.RefreshAVG_TURNS();
            return IsLoading;
        }
        //public bool IsLoaded
        //{
        //    get
        //    {                
        //        return _IsLoaded;
        //    }
        //    set
        //    {
        //        _IsLoaded = !this.Any(t => t.IsLoaded == false);
        //        if (_IsLoaded)
        //        {
        //            OWNER.SetBusyIndicatorState(false);
        //            IsLoading = false;
        //        }                
        //        NotifyPropertyChanged();
        //    }
        //}

        public MainWindow OWNER
        {
            get
            {
                return _OWNER;
            }
            set
            {
                _OWNER = value;
            }
        }

        public bool MinimizeWin { get; set; }

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

        #region IDisposable Members
        // Flag: Has Dispose already been called? 
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
                this.Update(t => t.CONTENT = null);
                this.Where(t => t != null).Update(t => t.Dispose());
                this.Where(t => t != null).Update(t => t = null);
                
                if (this.FOCUSEDTAB != null)
                {
                    this.FOCUSEDTAB.Dispose();
                    this.FOCUSEDTAB = null;
                }
            }

            // Free any unmanaged objects here. 
            //
            //get rid of each dailydata holiday collection 


            disposed = true;
        }
        #endregion
    }
}
