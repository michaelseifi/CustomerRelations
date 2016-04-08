using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Interfaces;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Sku : ISku, INotifyPropertyChanged
    {
        public Sku()
        {

        }

        #region properties
        public string DISPLAY_VALUE { get { return _SKUID; } }
        public string SELECTED_VALUE { get { return _SKUID; } }

        #endregion


        #region ISku Members

        private Setting _SETTING;
        private string _SELECTED_VALUE;
        private string _DISPLAY_VALUE;
        private string _SHIPTO;
        private string _SKUID;

        public Setting SETTING
        {
            get { return ((ISku)this).SETTING; }
        }
        Setting ISku.SETTING
        {
            get
            {
                return _SETTING;
            }
            set
            {
                _SETTING = value;
                NotifyPropertyChanged();
            }
        }
        string ISku.SKUID
        {
            get
            {
                return _SKUID;
            }
            set
            {
                if (_SKUID == value)
                    return;
                _SKUID = value;
            }
        }

        string ISku.SHIPTO
        {
            get
            {
                return _SHIPTO;
            }
            set
            {
                _SHIPTO = value;
            }
        }
  
        string ISku.DISPLAY_VALUE
        {
            get
            {
                return _SKUID;
            }
            set
            {
                _DISPLAY_VALUE = value;
            }
        }

        string ISku.SELECTED_VALUE
        {
            get
            {
                return _SKUID;
            }
            set
            {
                _SELECTED_VALUE = value;
            }
        }
        
        #endregion

        public override string ToString()
        {
            var s = ((ISku)this).SKUID;
            return s;
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


        public class Setting:INotifyPropertyChanged
        {
            private Setting() { }

            public Setting(int customerId, string skuId)
            {
                CUSTOMER_ID = customerId;
                SKU_ID = skuId;
            }

            #region properties
            public double SAFETY_STOCK_DAYS { get; set; }

            private string _ITEM_NUM;
            private string _COMMENT;
            private string _STORE_COUNT;
            private double _AVG_PRICE_POINT;
            public double AVG_PRICE_POINT
            {
                get
                {
                    return Math.Round(_AVG_PRICE_POINT, 2);
                }
                set
                {
                    _AVG_PRICE_POINT = value;
                    //var mainCustFocused = (Customer)MainWindow.myUsers.FocusedEmplid.CUSTOMERS.FocusedCustomer;
                    //var mainSkuFocused = mainCustFocused.FocusedSku;
                    NotifyPropertyChanged();
                }
            }
            
            public string STORE_COUNT
            {
                get
                {
                    return _STORE_COUNT;
                }
                set
                {
                    if (!string.IsNullOrEmpty(value))
                        _STORE_COUNT = value;
                    else
                        _STORE_COUNT = string.Empty;
                    NotifyPropertyChanged();
                }
            }

            public string COMMENT
            {
                get
                {
                    return _COMMENT;
                }
                set
                {
                    _COMMENT = value;
                    NotifyPropertyChanged();
                }
            }

            public string ITEM_NUM
            {
                get
                {
                    return _ITEM_NUM;
                }
                set
                {
                    _ITEM_NUM = value;
                    NotifyPropertyChanged();
                }
            }
            public int CUSTOMER_ID { get; set; }

            public string SKU_ID { get; set; }
            #endregion

            public void Save()
            {
                if (!Tools.IsInEditMode()) return;

                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                    {
                        var items = context.FORECASTER_CUSTOMER_SKU_SETTINGS.Where(i => i.CUSTOMER_ID == this.CUSTOMER_ID && i.SKU == this.SKU_ID);
                        if (items != null && items.Count() > 0)
                            foreach (var i in items)
                            {
                                i.SAFETY_STOCK_DAYS = this.SAFETY_STOCK_DAYS;
                                i.AVG_PRICE_POINT = this.AVG_PRICE_POINT;
                                i.STORE_COUNT = this.STORE_COUNT;
                                i.COMMENT = this.COMMENT;
                                i.ITEM_NUM = this.ITEM_NUM;
                                i.LAST_UPDATED = DateTime.Now;
                                i.ENTERED_BY = Environment.UserName;
                            }
                        else
                        {
                            var item = new datastores.FORECASTER_CUSTOMER_SKU_SETTINGS
                            {
                                AVG_PRICE_POINT = this.AVG_PRICE_POINT,
                                CUSTOMER_ID = this.CUSTOMER_ID,
                                SAFETY_STOCK_DAYS = this.SAFETY_STOCK_DAYS,
                                SKU = this.SKU_ID,
                                STORE_COUNT = this.STORE_COUNT,
                                COMMENT = this.COMMENT,
                                ITEM_NUM = this.ITEM_NUM,
                                LAST_UPDATED = DateTime.Now,
                                ENTERED_BY = Environment.UserName,
                            };
                            context.FORECASTER_CUSTOMER_SKU_SETTINGS.Add(item);
                        }
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    LogManger.Insert1(ex, "Unable to save customer sku setting");
                    throw;
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

        public class Settings:ObservableCollection<Setting>
        {
            public Settings()
            {
                
            }
            public Settings(List<Setting> list)
                : base(list)
            {
                
            }
            public Settings(IEnumerable<Setting> collection)
                : base(collection)
            {
                
            }



            #region async actions
            /// <summary>
            /// CALL THIS WHEN FOCUSEDCUSTOMER IS BEING SET IN CUSTOMER SEARCH BAR
            /// </summary>
            public static async void GetSkuSettingsAsync()
            {
                MainWindow.myTopMenuViewModel.IsSkuSettingsEnabled = false;
                MainWindow.mySkuSettings = await _GetSettingsTask();
                if (MainWindow.mySkuSettings != null)
                {
                    if (MainWindow.mySkuSettings.Count() > 0)
                    {
                        var skuList = MainWindow.mySkuSettings.Select(x => x.CUSTOMER_ID);
                        var userList = MainWindow.myUsers.Where(x => x.CUSTOMERS.Any(c => skuList.Contains(int.Parse(c.ACCOUNTNUM))));                        
                        foreach (var user in userList)
                            foreach (var cust in user.CUSTOMERS)
                                foreach (var sku in cust.SKUS)
                                    sku.SETTING = MainWindow.mySkuSettings.Where(x => x.SKU_ID == sku.SKUID && x.CUSTOMER_ID == int.Parse(((ICustomer)cust).ACCOUNTNUM)).FirstOrDefault();
                        MainWindow.myTopMenuViewModel.IsSkuSettingsEnabled = true;
                    }                   
                }
                else
                {
                    LogManger.RaiseErrorMessage(new Message { MESSAGE = "System was unable to set customer sku settings" });
                    LogManger.Insert("GetSkuSettingsAsync", "Unable to set customer sky settings");
                }
            }

            private static Task<Settings> _GetSettingsTask()
            {
                return Task.Run<Settings>(() => _Get());
            }

            private static Settings _Get()
            {
                
                try
                {
                    List<datastores.FORECASTER_CUSTOMER_SKU_SETTINGS> list;
                    using (var context = new datastores.ForecasterEntities(null))
                    //new datastores.FORECASTER_CUSTOMER_SKU_SETTINGSTableAdapters.FORECASTER_CUSTOMER_SKU_SETTINGSTableAdapter())
                    {
                        list = context.FORECASTER_CUSTOMER_SKU_SETTINGS.ToList();
                    }
                    if (list != null && list.Count() > 0)
                        return new Settings(list.Select(x => new Setting(x.CUSTOMER_ID, x.SKU)
                        {
                            AVG_PRICE_POINT = x.AVG_PRICE_POINT ?? 0,
                            SAFETY_STOCK_DAYS = x.SAFETY_STOCK_DAYS ?? 0,
                            STORE_COUNT = x.STORE_COUNT,
                            COMMENT = x.COMMENT ?? string.Empty,
                            ITEM_NUM = x.ITEM_NUM ?? string.Empty
                        }));
                    else
                        return new Settings();
                }
                catch (Exception ex)
                {
                    LogManger.Insert1(ex, "Unable to get customer sku setting collection");
                    return null;
                }
            }
            #endregion

        }
    }
}

