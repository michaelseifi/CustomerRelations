using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Parent = daisybrand.forecaster;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Objects;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Extensions;
namespace daisybrand.forecaster.Controlers.ViewModels
{
    public class TopMenu:INotifyPropertyChanged
    {
        public TopMenu ()
        {
        }
        public event EventHandler OPEN_FULL_VIEW_BTN_CLICKED;
        public event EventHandler QUICK_VIEW_BTN_CLICKED;
        private string _TOPTEXT;
        private bool _IS_OK_BTN_ENABLED;
        private bool _IsSkuSettingsEnabled;
        //private ITab _FOCUSED_TAB;
        private bool _TRACK_STEPS;
        private bool _TRACK_EVENTS;
        private string _AVG_PRICE_POINT_DOLLAR;
        //private ISku _FOCUSED_SKU;
        //private double _AVG_PRICE_POINT;
        //private string _AVERAGE_TURN_L4WK;
        //private string _AVERAGE_TURN;
        private bool _IsActualOrderRereshing;
        private string _TITLE;
        //private bool _FOCUSED_CUSTOMER_HAS_COMMENT;        
        //private ICustomer _FOCUSED_CUSTOMER;
        private string _STORE_COUNT;
        private string _STORE_COUNT_SKU;
        private bool _IS_ADMIN;
        private bool _IS_USER_SELECTED;
        private bool _IS_CUSTOMER_COLLECTION_LOADED;
        private bool _IS_DATA_LOEADED;
        private string _SEARCH_VALUE;
        private bool _Is_WEEKLY_POS_CASES_Enabled;
        private CustomerCollection _CUSTOMERS;
        private bool _Is_WEEKLYQS_ORDERQUANTITY_Enabled;
        private bool _Is_DAILYQA_WEEKLYQS_Enabled;
        private bool _Is_ALL_GRAPH_Enabled;
        private Version _APPLICATION_VERSION;


 
        #region settings
        public bool TRACK_EVENTS
        {
            get
            {
                return _TRACK_EVENTS;
            }
            set
            {
                _TRACK_EVENTS = value;
                NotifyPropertyChanged();
            }
        }

        public bool TRACK_STEPS
        {
            get
            {
                return _TRACK_STEPS;
            }
            set
            {
                _TRACK_STEPS = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public string TITLE
        {
            get
            {
                return _TITLE;
            }
            set
            {
                _TITLE = value;
                NotifyPropertyChanged();
            }
        }

        public string TOPTEXT
        {
            get
            {
                return _TOPTEXT;
            }
            set
            {
                _TOPTEXT = value;
                NotifyPropertyChanged();
            }
        }
        
        public bool IS_USER_SELECTED
        {
            get
            {
                return _IS_USER_SELECTED;
            }
            set
            {
                if (_IS_USER_SELECTED == value) return;
                _IS_USER_SELECTED = value;
                NotifyPropertyChanged();
            }
        }

        public bool IS_OK_BTN_ENABLED
        {
            get
            {
                return _IS_OK_BTN_ENABLED;
            }
            set
            {
                _IS_OK_BTN_ENABLED = value;
                NotifyPropertyChanged();
            }
        }

        public bool IS_ADMIN
        {
            get
            {
                return _IS_ADMIN;
            }
            set
            {
                _IS_ADMIN = value;
                NotifyPropertyChanged();
            }
        }

        public bool IS_DATA_LOEADED
        {
            get
            {
                return _IS_DATA_LOEADED;
            }
            set
            {
                if (_IS_DATA_LOEADED == value) return;
                _IS_DATA_LOEADED = value;
                NotifyPropertyChanged();
            }
        }

        public bool IS_CUSTOMER_COLLECTION_LOADED
        {
            get
            {
                return _IS_CUSTOMER_COLLECTION_LOADED;
            }
            set
            {
                if (_IS_CUSTOMER_COLLECTION_LOADED == value) return;
                _IS_CUSTOMER_COLLECTION_LOADED = value;
                NotifyPropertyChanged();
            }
        }

        public Version APPLICATION_VERSION
        {
            get
            {
                return _APPLICATION_VERSION;
            }
            set
            {
                if (value == _APPLICATION_VERSION) return;
                _APPLICATION_VERSION = value;
                NotifyPropertyChanged();
            }
        }

        public bool Is_ALL_GRAPH_Enabled
        {
            get
            {
                return _Is_ALL_GRAPH_Enabled;
            }
            set
            {
                if (_Is_ALL_GRAPH_Enabled == value) return;
                _Is_ALL_GRAPH_Enabled = MainWindow.GetTABS() != null ? value : false;
                NotifyPropertyChanged();
            }
        }

        public bool Is_WEEKLY_POS_CASES_Enabled
        {
            get
            {
                return _Is_WEEKLY_POS_CASES_Enabled;
            }
            set
            {
                if (_Is_WEEKLY_POS_CASES_Enabled == value) return;
                _Is_WEEKLY_POS_CASES_Enabled = MainWindow.GetTABS() != null ? value : false;
                NotifyPropertyChanged();
            }
        }
        
        public bool Is_WEEKLYQS_ORDERQUANTITY_Enabled
        {
            get
            {
                return _Is_WEEKLYQS_ORDERQUANTITY_Enabled;
            }
            set
            {
                if (_Is_WEEKLYQS_ORDERQUANTITY_Enabled == value) return;
                _Is_WEEKLYQS_ORDERQUANTITY_Enabled = MainWindow.GetTABS() != null 
                    && MainWindow.GetTABS().FOCUSEDTAB != null 
                    && (MainWindow.GetTABS().FOCUSEDTAB.myDailyData != null ? value : false);
                NotifyPropertyChanged();
            }
        }

        public bool Is_DAILYQA_WEEKLYQS_Enabled
        {
            get
            {
                return _Is_DAILYQA_WEEKLYQS_Enabled;
            }
            set
            {
                if (_Is_DAILYQA_WEEKLYQS_Enabled == value) return;
                _Is_DAILYQA_WEEKLYQS_Enabled = MainWindow.GetTABS() != null 
                    && MainWindow.GetTABS().FOCUSEDTAB != null
                    && (MainWindow.GetTABS().FOCUSEDTAB.myDailyData != null ? value : false);
                NotifyPropertyChanged();
            }
        }

        //private bool _FOCUSED_CUSTOMERchanging;

        public string STORE_COUNT
        {
            get
            {
                if (MainWindow.GetTABS() != null)
                {
                    var tabs = MainWindow.GetTABS();
                    return tabs.FOCUSEDCUSTOMER.SETTING.STORE_COUNT;
                }
                else if (daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null
                    && daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING != null)
                    return daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING.STORE_COUNT;
                return null;                
            }
            set
            {
                _STORE_COUNT = value;
                if (MainWindow.GetTABS() != null)
                {

                    MainWindow.GetTABS().FOCUSEDCUSTOMER.SETTING.STORE_COUNT = value;
                    daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING.STORE_COUNT = value;
                    MainWindow.GetTABS().FOCUSEDCUSTOMER.SETTING.Save();
                }
                else if (daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null
                    && daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING != null)
                {
                    daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING.STORE_COUNT = value;
                    daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.SETTING.Save();
                }

                //if (!_FOCUSED_CUSTOMERchanging)
                //{
                //    if (FOCUSED_CUSTOMER.SETTING == null)
                //        FOCUSED_CUSTOMER.SETTING = new Objects.Customer.Setting()
                //        {
                //            CUSTOMER_ID = int.Parse(FOCUSED_CUSTOMER.ACCOUNTNUM)
                //        };
                //    FOCUSED_CUSTOMER.SETTING.STORE_COUNT = value;
                //    //save it to the database if the app is not in readonly mode

                //    FOCUSED_CUSTOMER.SETTING.Save();
                //}                
                //_FOCUSED_CUSTOMERchanging = false;
                NotifyPropertyChanged();
            }
        }

        public string STORE_COUNT_SKU
        {
            get
            {
                if (MainWindow.GetTABS() != null)
                {
                    var t = MainWindow.GetTABS();
                    return t.FOCUSEDTAB.SKU.SETTING.STORE_COUNT;
                }
                //else if (daisybrand.forecaster.MainWindow.myCustomerSearchViewModel != null
                //    && daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null
                //    && daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.FocusedSku.SETTING != null)
                //    return daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.FocusedSku.SETTING.STORE_COUNT;
                return null;
            }
            set
            {
                _STORE_COUNT_SKU = value;
                if (MainWindow.GetTABS() != null)
                {
                    MainWindow.GetTABS().FOCUSEDCUSTOMER.FocusedSku.SETTING.STORE_COUNT = value;
                    daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.FocusedSku.SETTING.STORE_COUNT = value;
                    MainWindow.GetTABS().FOCUSEDCUSTOMER.FocusedSku.SETTING.Save();

                }
                NotifyPropertyChanged("STORE_COUNT_SKU");
            }           
        }
        //public void SetSTORE_COUNT(string value)
        //{
        //    _STORE_COUNT = value;
        //    RefreshSTORE_COUNT();
        //}

        public void RefreshSkuSettings()
        {
            RefreshAVG_POINT();
            RefreshAVG_TURNS();
            RefreshSTORE_COUNT_SKU();
            //RefreshDAYS_OF_SUPPLIES();

        }
        public void RefreshSTORE_COUNT()
        {
            NotifyPropertyChanged("STORE_COUNT");
        }

        private void RefreshSTORE_COUNT_SKU()            
        {
            NotifyPropertyChanged("STORE_COUNT_SKU");
        }

        private void RefreshAVG_POINT()
        {
            _AVG_PRICE_POINT_DOLLAR = null;
            NotifyPropertyChanged("AVG_PRICE_POINT_DOLLAR");
        }

        //public double AVG_PRICE_POINT
        //{
        //    get
        //    {
        //        return Math.Round(_AVG_PRICE_POINT, 2);
        //    }
        //    set
        //    {
        //        _AVG_PRICE_POINT = value;
        //        _AVG_PRICE_POINT_DOLLAR = Convert.ToDecimal(value);
        //        NotifyPropertyChanged();
        //        NotifyPropertyChanged("AVG_PRICE_POINT_DOLLAR");
        //    }
        //}


        public string AVG_PRICE_POINT_DOLLAR
        {
            get
            {
                if (_AVG_PRICE_POINT_DOLLAR != null) return _AVG_PRICE_POINT_DOLLAR;
                if (MainWindow.GetTABS() != null)
                {
                    var t = MainWindow.GetTABS();
                    return t.FOCUSEDTAB.SKU.SETTING.AVG_PRICE_POINT.ToString(); //.ToString("C2");
                }
                //else if (daisybrand.forecaster.MainWindow.myCustomerSearchViewModel != null
                //    && daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER != null
                //    && daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.FocusedSku.SETTING != null)
                //    return daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.SELECTED_CUSTOMER.FocusedSku.SETTING.STORE_COUNT;
                return null;
                //return _AVG_PRICE_POINT_DOLLAR.ToString("C2");
            }
            set
            {
                decimal d;
                if (decimal.TryParse(value, out d))
                {
                    //.Replace("$", "");
                    try
                    {

                        _AVG_PRICE_POINT_DOLLAR = value;
                        if (MainWindow.GetTABS() != null && MainWindow.GetTABS().FOCUSEDTAB != null)
                        {
                            Parent.MainWindow.mySkuSettings.Where(x => x.SKU_ID == MainWindow.GetTABS().FOCUSEDTAB.SKU.SKUID && x.CUSTOMER_ID == int.Parse(Parent.MainWindow.myMainWindowViewModel.TABS.FOCUSEDCUSTOMER.ACCOUNTNUM))
                            .Update(x => x.AVG_PRICE_POINT = Convert.ToDouble(d));
                            MainWindow.GetTABS().FOCUSEDTAB.SKU.SETTING.AVG_PRICE_POINT = Convert.ToDouble(d);
                            MainWindow.GetTABS().FOCUSEDTAB.SKU.SETTING.Save();
                        }

                        NotifyPropertyChanged();
                        NotifyPropertyChanged("AVG_PRICE_POINT");
                        NotifyPropertyChanged("FOCUSED_SKU");
                    }
                    catch (NullReferenceException) { }
                    catch (Exception ex)
                    {
                        LogManger.Insert1(ex, "Entering AVG price point");
                        LogManger.RaiseErrorMessage(new Message { MESSAGE = String.Format("Enter a valid value. {0} is not a valid value.", value) });
                        NotifyPropertyChanged();
                    }
                }

            }
        }

        //public ICustomer FOCUSED_CUSTOMER
        //{
        //    get
        //    {
        //        return _FOCUSED_CUSTOMER;
        //    }
        //    set
        //    {
        //        _FOCUSED_CUSTOMER = value;
        //        if (Parent.MainWindow.myMainWindowViewModel != null)
        //            Parent.MainWindow.myMainWindowViewModel.IsThereCustomerComment = !string.IsNullOrEmpty(value.SETTING.COMMENT);
        //        FOCUSED_CUSTOMER_HAS_COMMENT = value !=null && value.SETTING != null && !string.IsNullOrEmpty(value.SETTING.COMMENT);
        //        _FOCUSED_CUSTOMERchanging = true;
        //        if (value != null && value.SETTING != null)
        //            STORE_COUNT = value.SETTING.STORE_COUNT;                                    
        //        else
        //            STORE_COUNT = null;                    
        //        NotifyPropertyChanged();
        //        NotifyPropertyChanged("IsCustomerFocused");
        //    }
        //}

        /// <summary>
        /// used to enable AV_PRICE_POINT textbox
        /// </summary>
        public bool IsCustomerFocused
        {
            get
            {
                return MainWindow.GetTABS() != null;// FOCUSED_CUSTOMER != null;
            }
        }

        //public ISku FOCUSED_SKU
        //{
        //    get
        //    {
        //        return _FOCUSED_SKU;
        //    }
        //    set
        //    {
        //        _FOCUSED_SKU = value;
        //        if (value != null)
        //        {
        //            if (value.SETTING == null) value.SETTING = new Objects.Sku.Setting(int.Parse(FOCUSED_CUSTOMER.ACCOUNTNUM), value.SKUID);
        //            AVG_PRICE_POINT = value.SETTING.AVG_PRICE_POINT;
        //        }
        //        NotifyPropertyChanged();
        //        NotifyPropertyChanged("IsSkuFocused");
        //    }
        //}

        /// <summary>
        /// used to enable AVG_PRICE_POINT textbox
        /// </summary>
        public bool IsSkuFocused
        {
            get
            {
                return MainWindow.GetTABS() != null && MainWindow.GetTABS().FOCUSEDTAB != null;
            }
        }

        /// <summary>
        /// used to enable AVG_PRICE_POINT textbox
        /// </summary>
        public bool IsSkuSettingsEnabled
        {
            get
            {
                return _IsSkuSettingsEnabled;
            }
            set
            {
                _IsSkuSettingsEnabled = value;
                NotifyPropertyChanged("");
            }
        }

        //public ITab FOCUSED_TAB
        //{
        //    get
        //    {
        //        return _FOCUSED_TAB;
        //    }
        //    set
        //    {
        //        _FOCUSED_TAB = value;
        //        if (value != null)
        //        {
        //            FOCUSED_SKU = value.SKU;
        //            if (Parent.MainWindow.myCustomerSearchViewModel != null)
        //                Parent.MainWindow.myCustomerSearchViewModel.SELECTED_SKU = value.SKU as Sku;
        //            NotifyPropertyChanged("AVERAGE_TURN_L4WK");
        //            NotifyPropertyChanged("AVERAGE_TURN");
        //            NotifyPropertyChanged("STORE_COUNT");
        //        }
        //        NotifyPropertyChanged();
        //    }
        //}

        //public bool FOCUSED_CUSTOMER_HAS_COMMENT
        //{
        //    get
        //    {
        //        return _FOCUSED_CUSTOMER_HAS_COMMENT;
        //    }
        //    set
        //    {
        //        _FOCUSED_CUSTOMER_HAS_COMMENT = value;                
        //        NotifyPropertyChanged();
        //    }
        //}

        public string AVERAGE_TURN
        {
            get
            {
                if (MainWindow.GetTABS() == null || MainWindow.GetTABS().FOCUSEDTAB == null || MainWindow.GetTABS().FOCUSEDTAB.myWeeklyData == null) return null;
                return Math.Round(MainWindow.GetTABS().FOCUSEDTAB.myWeeklyData.AVERAGE_TURNS, 0).ToString(); 
            }
            //set
            //{
            //    _AVERAGE_TURN = value;
            //    NotifyPropertyChanged();
            //}
        }

        public string AVERAGE_TURN_L4WK
        {
            get
            {
                if (MainWindow.GetTABS() == null 
                    || MainWindow.GetTABS().FOCUSEDTAB.myWeeklyData == null)
                    return null;
                return 
                    Math.Round(
                        MainWindow.GetTABS().FOCUSEDTAB.myWeeklyData.AVERAGE_TURNS_L4WK, 0).ToString();
            }
            //set
            //{
            //    _AVERAGE_TURN_L4WK = value;
            //    NotifyPropertyChanged();
            //}
        }

        public decimal DAYS_OF_SUPPLIES
        {
            get
            {
                if (MainWindow.GetTABS() == null
                    || MainWindow.GetTABS().FOCUSEDTAB.myWeeklyData == null)
                    return 0;
                var value = MainWindow.GetTABS().FOCUSEDTAB.myWeeklyData.DAYS_OF_SUPPLIES;
                return value;
                
            }
        }        
        
        //public void RefreshDAYS_OF_SUPPLIES ()
        //{
        //    NotifyPropertyChanged("DAYS_OF_SUPPLIES");
        //}
        public void RefreshAVG_TURNS()
        {
            NotifyPropertyChanged("AVERAGE_TURN");
            NotifyPropertyChanged("AVERAGE_TURN_L4WK");
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

        public string SEARCH_VALUE
        {
            get
            {
                return _SEARCH_VALUE;
            }
            set
            {
                _SEARCH_VALUE = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsActualOrderRereshing
        {
            get
            {
                return _IsActualOrderRereshing;
            }
            set
            {
                _IsActualOrderRereshing = value;
                daisybrand.forecaster.MainWindow.myMainWindowViewModel.IsActualOrderRereshing = value;
                NotifyPropertyChanged();
                daisybrand.forecaster.MainWindow.myCustomerSearchViewModel.NotifyPropertyChanged("IS_OK_BTN_ENABLED");
            }
        }

        public static void EnableGraphs()
        {
            _EnableGraphs(true);
        }

        public static void DisableGraphs()
        {
            _EnableGraphs(false);
        }

        private static void _EnableGraphs(bool isEnabled)
        {
            if (Parent.MainWindow.myTopMenuViewModel != null)
            {
                Parent.MainWindow.myTopMenuViewModel.Is_ALL_GRAPH_Enabled =
                    Parent.MainWindow.myTopMenuViewModel.Is_DAILYQA_WEEKLYQS_Enabled = 
                    Parent.MainWindow.myTopMenuViewModel.Is_WEEKLYQS_ORDERQUANTITY_Enabled = isEnabled;
            }
        }

        //public void SetCustomers(IEnumerable<string> customers)
        //{
        //    CUSTOMERS = customers.Cast<object>().ToList();
        //}

        /// <summary>
        /// ADDS LIST OF ALL CUSTOMERS TO THE CUSTOMER PROPERTY OF THE TOPMENU MODEL
        /// </summary>
        public static async void GetListOfCustomersAsync()
        {
            var a = await _GetListOfCustomersTask(10);
            if (!a)
                throw new ApplicationException("Unable to retrieve user's skus");
        }

        private static Task<bool> _GetListOfCustomersTask(int seconds)
        {
            return Task.Run<bool>(() => _GetListOfCustomers(seconds));
        }

        private static bool _GetListOfCustomers(int seconds)
        {
            try
            {
                Parent.MainWindow.myTopMenuViewModel.CUSTOMERS = 
                    new CustomerCollection(
                        Parent.MainWindow.myUsers
                            .SelectMany(x => x.CUSTOMERS)
                            .OrderBy(x => x.ACCOUNT_NAME));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SetTitle(string title)
        {
            if (daisybrand.forecaster.MainWindow.myTopMenuViewModel != null)
            {
                daisybrand.forecaster.MainWindow.myTopMenuViewModel.TITLE = title;
                daisybrand.forecaster.MainWindow.myTopMenuViewModel.NotifyPropertyChanged("TITLE");
            }
        }

        public void FullViewBtnClicked()
        {
            if (OPEN_FULL_VIEW_BTN_CLICKED != null)
                OPEN_FULL_VIEW_BTN_CLICKED(this, EventArgs.Empty);
        }

        public void QuickViewBtnClicked()
        {
            if (QUICK_VIEW_BTN_CLICKED != null)
                QUICK_VIEW_BTN_CLICKED(this, EventArgs.Empty);
        }

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
