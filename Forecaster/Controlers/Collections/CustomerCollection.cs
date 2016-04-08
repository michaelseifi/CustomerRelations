using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataStores = CRDataStore;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Controlers.Interfaces;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class CustomerCollection : 
        ObservableCollection<ICustomer>, INotifyPropertyChanged
    {
        public CustomerCollection()
        {
            
        }
        public CustomerCollection(List<ICustomer> list)
            : base(list)
        {
            
        }
        public CustomerCollection(IEnumerable<ICustomer> collection)
            : base(collection)
        {
            
        }

        public void AddRange(List<ICustomer> list)
        {
            foreach (ICustomer c in list)
            {
                this.Add(c);
            }
        }

        #region properties
        //private Customer _FocusedCustomer;
        //public Customer FocusedCustomer
        //{
        //    get
        //    {
        //        return _FocusedCustomer;
        //    }
        //    set
        //    {
        //        _FocusedCustomer = value;
        //        //MainWindow.myTopMenuViewModel.FOCUSED_CUSTOMER = value;
        //        MainWindow.myStatusBarViewModel.ADD_URL = value == null || value.SETTING == null ? null : value.SETTING.ADD_URL_URI;
        //        //value.GetSettingAsync();
        //        NotifyPropertyChanged();
        //    }
        //}

        #endregion

        #region statics
        public static IEnumerable<string> GetListFrom852()
        {
            using(var context = 
                new DataStores.dbidbEntities())
            {
                return context.VMI_Get_Customers().ToArray();
            }        
        }

        public static List<int> GetListOfExceptions()
        {
            List<int> list;
            using (var context = new DataStores.ForecasterEntities(null))
                //var dt = new DataStores.FORECASTER_CUSTOMER_EXCEPTIONSTableAdapters.FORECASTER_CUSTOMER_EXCEPTIONSTableAdapter())
            {
                list = context.FORECASTER_CUSTOMER_EXCEPTIONS.Select(i => i.CUSTOMER_ID).ToList(); 
                //dt.GetData().Select(x => x.CUSTOMER_ID);
            }
            return list;
        }
#endregion


        public void AddToExceptions(int customerNumber)
        {
            if (!Tools.IsInEditMode()) return; 
            using (var context = new DataStores.ForecasterEntities(null))
            {
                context.FORECASTER_CUSTOMER_EXCEPTIONS.Add(new DataStores.FORECASTER_CUSTOMER_EXCEPTIONS
                {
                    CUSTOMER_ID = customerNumber,
                });
                context.SaveChanges();
            }
        }

        public void RemoveFromExceptions(int customerNumber)
        {
            if (!Tools.IsInEditMode()) return;  
            try
            {
                using (var context = new DataStores.ForecasterEntities(null))                
                {
                    var item = context.FORECASTER_CUSTOMER_EXCEPTIONS.First(i => i.CUSTOMER_ID == customerNumber);
                    context.FORECASTER_CUSTOMER_EXCEPTIONS.Remove(item);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to delete a customer from customer exception list");
            }
        }

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
