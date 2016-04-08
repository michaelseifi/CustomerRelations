using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datastores = CRDataStore;
using daisybrand.forecaster.Controlers.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Extensions;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class Customer : ICustomer, INotifyPropertyChanged
    {

        public Customer()
        {
            _SKUS = new Collections.SkuCollection();
            _SKUS_EXCEPTIONS = new Collections.SkuCollection();
        }

        #region ICustomer Members
        private ISku _FocusedSku;
        private bool _HAS_COMMENT;
        private Setting _SETTING;

        private Collections.SkuCollection _SKUS_EXCEPTIONS;
        private string _CR_ASSOCIATE;
        private string _REGIONAL_MANGER;
        private string _CLASSIFICATION_ID;
        private string _COMPANY_NAME;
        private Collections.SkuCollection _SKUS;
        private string _SELECTED_VALUE;
        private string _DISPLAY_VALUE;
        private int _PARTYID;
        private string _ACCOUNTNAME;
        private string _EMPLID;
        private string _CITY;
        private string _ZIPCODE;
        private string _ADDRESS;
        private string _INVENTLOCATION;
        private string _DLVTERM;
        private string _PAYMDAYID;
        private string _PAYMTERMID;
        private string _CUSTGROUP;
        private string _INVOICEACCOUNT;
        private string _NAME;
        private string _ACCOUNTNUM;




        public Setting SETTING
        {
            get {

                return ((ICustomer)this).SETTING; }
        }

        public void SetSETTING(Setting setting)
        {
            ((ICustomer)this).SETTING = setting;
        }
        public bool HAS_COMMENT
        {
            get
            {
                return _HAS_COMMENT;
            }
            set
            {
                _HAS_COMMENT = value;
                NotifyPropertyChanged();
            }
        }

        public SkuCollection SKU_EXCEPTIONS_COLLECTION
        {
            get
            {
                return ((ICustomer)this).SKUS_EXCEPTIONS;
            }
        }
        public SkuCollection SKU_COLLECTION
        {
            get
            {
                return ((ICustomer)this).SKUS;
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
                //if (_DISPLAY_VALUE == value) return;
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
                //if (_SELECTED_VALUE == value) return;
                _SELECTED_VALUE = value;
                NotifyPropertyChanged();
            }
        }

        public ISku FocusedSku
        {
            get
            {
                return _FocusedSku;
            }
            set
            {   
                ((ICustomer)this).FocusedSku = value;
               
                //delay notifypropertychanged to icustomer.focusedsku
            }
        }

        ISku ICustomer.FocusedSku
        {
            get
            {
                return _FocusedSku;
            }
            set
            {
                _FocusedSku = value;
                //RESET STORE COUNT SKU
                if (value != null)
                    MainWindow.myTopMenuViewModel.RefreshSkuSettings();
                //MainWindow.myTopMenuViewModel.FOCUSED_SKU = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FocusedSku");
            
            }
        }

        Setting ICustomer.SETTING
        {
            get
            {
                if (_SETTING == null)
                {
                    _SETTING = new Setting
                    {
                        CUSTOMER_ID = int.Parse(_ACCOUNTNUM),
                        REPORT_START_DAY = "none",
                        REPORT_END_DAY = "none"
                    };

                    _SETTING.Save();
                    LogManger.RaiseErrorMessage(new Message
                    {
                        MESSAGE =
                            "System added an empty record for this customer's settings.\rGo to Edit>Customer settings and edit this customer's settings."
                    });
                }

                return _SETTING;
            }
            set
            {
                _SETTING = value;
                NotifyPropertyChanged();
                if (value != null && !string.IsNullOrEmpty(value.COMMENT))
                    this.HAS_COMMENT = true;
                else
                    this.HAS_COMMENT = false;                
            }
        }



        string ICustomer.ACCOUNTNUM
        {
            get
            {
                return _ACCOUNTNUM;
            }
            set
            {
                if (_ACCOUNTNUM == value) return;
                _ACCOUNTNUM = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.NAME
        {
            get
            {
                return _NAME;
            }
            set
            {
                if (_NAME == value) return;
                _NAME = value;
                NotifyPropertyChanged();

            }
        }

        string ICustomer.INVOICEACCOUNT
        {
            get
            {
                return _INVOICEACCOUNT;
            }
            set
            {
                if (_INVOICEACCOUNT == value) return;
                _INVOICEACCOUNT = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.CUSTGROUP
        {
            get
            {
                return _CUSTGROUP;
            }
            set
            {
                if (_CUSTGROUP == value) return;
                _CUSTGROUP = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.PAYMTERM_ID
        {
            get
            {
                return _PAYMTERMID;
            }
            set
            {
                if (_PAYMTERMID == value) return;
                _PAYMTERMID = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.PAYMDAY_ID
        {
            get
            {
                return _PAYMDAYID;
            }
            set
            {
                if (_PAYMDAYID == value) return;
                _PAYMDAYID = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.DLVTERM
        {
            get
            {
                return _DLVTERM;
            }
            set
            {
                if (_DLVTERM == value) return;
                _DLVTERM = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.INVENT_LOCATION
        {
            get
            {
                return _INVENTLOCATION;
            }
            set
            {
                if (_INVENTLOCATION == value) return;
                _INVENTLOCATION = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.ADDRESS
        {
            get
            {
                return _ADDRESS;
            }
            set
            {
                if (_ADDRESS == value) return;
                _ADDRESS = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.ZIPCODE
        {
            get
            {
                return _ZIPCODE;
            }
            set
            {
                if (_ZIPCODE == value) return;
                _ZIPCODE = value;
                NotifyPropertyChanged();
            }
        }

        string ICustomer.CITY
        {
            get
            {
                return _CITY;
            }
            set
            {
                if (_CITY == value) return;
                _CITY = value;
                NotifyPropertyChanged();
            }
        }
        string ICustomer.EMPLID
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

        string ICustomer.ACCOUNT_NAME
        {
            get
            {
                return _ACCOUNTNAME;
            }
            set
            {
                if (_ACCOUNTNAME == value) return;
                _ACCOUNTNAME = value;
                NotifyPropertyChanged();
            }
        }

        int ICustomer.PARTYID
        {
            get
            {
                return _PARTYID;
            }
            set
            {
                if (_PARTYID == value) return;
                _PARTYID = value;
                NotifyPropertyChanged();
            }
        }

        Collections.SkuCollection ICustomer.SKUS
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

        

        string ICustomer.COMPANY_NAME
        {
            get
            {
                return _COMPANY_NAME;
            }
            set
            {
                _COMPANY_NAME = value;
            }
        }

        string ICustomer.CLASSIFICATION_ID
        {
            get
            {
                return _CLASSIFICATION_ID;
            }
            set
            {
                _CLASSIFICATION_ID = value;
            }
        }

        string ICustomer.REGIONAL_MANGER
        {
            get
            {
                return _REGIONAL_MANGER;
            }
            set
            {
                _REGIONAL_MANGER = value;
            }
        }

        string ICustomer.CR_ASSOCIATE
        {
            get
            {
                return _CR_ASSOCIATE;
            }
            set
            {
                _CR_ASSOCIATE = value;
            }
        }


        Collections.SkuCollection ICustomer.SKUS_EXCEPTIONS
        {
            get
            {
                return _SKUS_EXCEPTIONS;
            }
            set
            {
                _SKUS_EXCEPTIONS = value;
                NotifyPropertyChanged();
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


        public override string ToString()
        {
            return DISPLAY_VALUE;
        }




        #region async actions
        /// <summary>
        /// CALL THIS WHEN FOCUSEDCUSTOMER IS BEING SET IN CUSTOMER SEARCH BAR
        /// </summary>
        public async void GetSettingAsync()
        {
            var s = await _GetSettingTask(int.Parse(this._ACCOUNTNUM));
            if (s != null)
            {
                ((ICustomer)this).SETTING = s;
               
                //if (MainWindow.myTopMenuViewModel != null)
                //{
                //    MainWindow.myTopMenuViewModel.STORE_COUNT = s.STORE_COUNT;
                //    //Uri uri;
                //    //if (Uri.TryCreate(((ICustomer)this).SETTING.ADD_URL, UriKind.Absolute, out uri))
                //    //    MainWindow.myStatusBarViewModel.ADD_URL = uri;
                //}
            }
            else
            {
                //MainWindow.myStatusBarViewModel.ADD_URL = null;
            }
        }

        private static Task<Setting> _GetSettingTask(int customerId)
        {
            return Task.Run<Setting>(() => _GetSetting(customerId));
        }

        private static Setting _GetSetting(int customerId)
        {            
            try
            {
                Setting s;
                using (var context = new datastores.ForecasterEntities(null))
                //var dt = new datastores.FORECASTER_CUSTOMER_SETTINGSTableAdapters.FORECASTER_CUSTOMER_SETTINGSTableAdapter())
                {
                    var a = context.FORECASTER_CUSTOMER_SETTINGS.First(i => i.CUSTOMER_ID == customerId);
                    s = new Setting
                    {
                        ADD_URL = a.LINK ?? "",
                        STORE_COUNT = a.STORE_COUNT ?? "",
                        CUSTOMER_ID = a.CUSTOMER_ID
                    };
                }
                return s;
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to get customer setting");
                return null;
            }
            
        }
        #endregion

        public class Setting : ReportingDay,   IEditableObject, INotifyPropertyChanged
        {

            #region properties
            private bool _WILL_COVER_ORDER_DATE;
            
            private string _COMMENT;
            
            private bool _HAS_CHANGED;
            private int _CUSTOMER_ID;
            private string _STORE_COUNT;
            private string _ADD_URL;
            public bool HAS_CHANGED
            {
                get
                {
                    return _HAS_CHANGED;
                }
                set
                {
                    _HAS_CHANGED = value;
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
            public Uri ADD_URL_URI
            {
                get
                {
                    Uri uri;
                    if (Uri.TryCreate(this.ADD_URL, UriKind.Absolute, out uri))
                        return uri;
                    return null;
                }                
            }
            public string ADD_URL
            {
                get
                {
                    return _ADD_URL;
                }
                set
                {
                    _ADD_URL = value;                    
                    //var tabs = Controlers.ViewModels.MainWindow.GetTABS();
                    //if (tabs != null)
                    //{
                    //    var mainCustFocused = tabs.FOCUSEDCUSTOMER;
                    //    if (mainCustFocused != null && int.Parse(mainCustFocused.ACCOUNTNUM) == this.CUSTOMER_ID)
                    //        MainWindow.myStatusBarViewModel.ADD_URL = ADD_URL_URI;
                    //}
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
                    if (_STORE_COUNT == value) return;
                    _STORE_COUNT = value;
                    //if (MainWindow.myMainWindowViewModel != null && MainWindow.myMainWindowViewModel.TABS != null && MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER != null)
                    //{
                    //    var mainCustFocused = MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER;
                    //    if (mainCustFocused != null && int.Parse(mainCustFocused.ACCOUNTNUM) == this.CUSTOMER_ID)
                    //        MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER = mainCustFocused;
                    //}
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
                    if (MainWindow.myMainWindowViewModel != null)
                        MainWindow.myMainWindowViewModel.SetIsThereCustomerComment(this);
                }
            }

            public bool WILL_COVER_ORDER_DATE
            {
                get
                {
                    return _WILL_COVER_ORDER_DATE;
                }
                set
                {
                    _WILL_COVER_ORDER_DATE = value;
                    NotifyPropertyChanged();
                }
            }

            #endregion

            public bool Save()
            {               
                this.EndEdit();
                if (!Tools.IsInEditMode()) return true;
                try
                {
                    using (var context = new datastores.ForecasterEntities(null))
                        //var dt = new datastores.FORECASTER_CUSTOMER_SETTINGSTableAdapters.FORECASTER_CUSTOMER_SETTINGSTableAdapter())
                    {
                        var items = context.FORECASTER_CUSTOMER_SETTINGS.Where(i => i.CUSTOMER_ID == this.CUSTOMER_ID);
                        if(items != null && items.Count()>0)
                            foreach (var i in items)
                            {
                                i.COMMENT = this.COMMENT.Substring(this.COMMENT.LastIndexOf(":") + 1);
                                i.LINK = this.ADD_URL;
                                i.STORE_COUNT = this.STORE_COUNT;
                                i.REPORTING_DAYS = this.REPORT_DAYS;
                                i.REPORTING_START = this.REPORT_START_DayOfWeek.ToString();
                                i.REPORTING_END = this.REPORT_END_DayOfWeek.ToString();
                                i.WILL_COVER_ORDER_DATE = this.WILL_COVER_ORDER_DATE;
                                i.LAST_UPDATED = DateTime.Now;
                                i.ENTERED_BY = Environment.UserName;
                            }
                        else
                        {
                            var item = new datastores.FORECASTER_CUSTOMER_SETTINGS
                            {
                                COMMENT = this.COMMENT,
                                CUSTOMER_ID = this.CUSTOMER_ID,
                                LINK = this.ADD_URL,
                                REPORTING_DAYS = this.REPORT_DAYS,
                                REPORTING_END = this.REPORT_END_DayOfWeek.ToString(),
                                REPORTING_START = this.REPORT_START_DayOfWeek.ToString(),
                                WILL_COVER_ORDER_DATE = this.WILL_COVER_ORDER_DATE,
                                LAST_UPDATED = DateTime.Now,
                                ENTERED_BY = Environment.UserName
                            };
                            context.FORECASTER_CUSTOMER_SETTINGS.Add(item);
                        }
                        context.SaveChanges();
                        //if (dt.UpdateQuery1(this.ADD_URL, this.STORE_COUNT, this.COMMENT, this.REPORT_DAYS, this.REPORT_START_DayOfWeek.ToString(), this.REPORT_END_DayOfWeek.ToString(), this.WILL_COVER_ORDER_DATE, this.CUSTOMER_ID) < 1)
                        //    dt.InsertQuery1(this.CUSTOMER_ID, this.ADD_URL, this.STORE_COUNT, this.COMMENT, this.REPORT_DAYS, this.REPORT_START_DayOfWeek.ToString(), this.REPORT_END_DayOfWeek.ToString(), this.WILL_COVER_ORDER_DATE);

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LogManger.Insert1(ex, "Unable to save customer settings");
                    return false;
                }
                
            }


            #region IEditableObject Members

            public void BeginEdit()
            {
                HAS_CHANGED = true;
            }

            public void CancelEdit()
            {
                throw new NotImplementedException();
            }

            public void EndEdit()
            {
                HAS_CHANGED = false;
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

        public class Settings : ObservableCollection<Setting>
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
            public static async void GetCustomersSettingsAsync()
            {
                MainWindow.myCustomerSettings = await _GetSettingsTask();
                if (MainWindow.myCustomerSettings != null)
                {
                    var customerList = MainWindow.myCustomerSettings.Select(x => x.CUSTOMER_ID);
                    var userList = MainWindow.myUsers.Where(x => x.CUSTOMERS.Any(c => customerList.Contains(int.Parse(c.ACCOUNTNUM))));
                    foreach (var user in userList)
                    {
                        foreach (var cust in user.CUSTOMERS)
                        {
                            ((ICustomer)cust).SETTING = MainWindow.myCustomerSettings.Where(x => x.CUSTOMER_ID == int.Parse(((ICustomer)cust).ACCOUNTNUM)).FirstOrDefault();
                        }
                    }
                }
            }

            private static Task<Settings> _GetSettingsTask()
            {
                return Task.Run<Settings>(() => _Get());
            }

            private static Settings _Get()
            {
                List<datastores.FORECASTER_CUSTOMER_SETTINGS> list;
                using (var context = new datastores.ForecasterEntities(null)) 
                    //new datastores.FORECASTER_CUSTOMER_SETTINGSTableAdapters.FORECASTER_CUSTOMER_SETTINGSTableAdapter())
                {
                    list = context.FORECASTER_CUSTOMER_SETTINGS.ToList();
                }
                if(list != null && list.Count() >0){
                    return new Settings(list.Select(x => new Setting
                    {
                        ADD_URL = x.LINK ?? "",
                        STORE_COUNT = x.STORE_COUNT ?? "",
                        CUSTOMER_ID = x.CUSTOMER_ID,
                        COMMENT = x.COMMENT ?? "",
                        REPORT_DAYS = x.REPORTING_DAYS ?? 0,
                        REPORT_END_DAY = x.REPORTING_END ?? "",
                        REPORT_START_DAY = x.REPORTING_START ?? "",
                        WILL_COVER_ORDER_DATE = x.WILL_COVER_ORDER_DATE ?? false,
                    }));
                }
                return new Settings();
            }
            #endregion


        }

    }
}
