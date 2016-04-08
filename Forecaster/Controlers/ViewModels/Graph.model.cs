using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Controlers.Interfaces;
using Parent = daisybrand.forecaster;
using System.Collections;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;
using System.Threading;
using WpfAnimatedGif;

namespace daisybrand.forecaster.Controlers.ViewModels
{
    public class Graph:INotifyPropertyChanged
    {
        ITab TAB { get; set; }
        private bool _Is_DAILY_DATA_COLLECTION_Loaded;
        private bool _Is_DAILY_DATA_COLLECTION_Loading;
        private bool _IS_ORDERCOLLECTION_LOADING;
        private object _WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT;
        private object _DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT;
        private object _PROMOTION_CONTENT;
        private object _DAILY_QA_AND_WEEKLY_QS_CONTENT;
        private object _WEEKLY_QS_ORDER_QUANTITY_CONTENT;
        private object _VOLATILITY_CONTENT;
        private object _PERCENTAGE_CONTENT;
        private object _DISTRIBUTION_CONTENT;
        
        
        public Graph(string customerName, string customerNumber , ITab tab)
        {
            TAB = tab;           

            
            VOLATILITY_NUMBER_OF_WEEKS = 17;
            DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS = 17;
            DAILY_QA_WEEKLY_QS_X_Range = TAB.myDailyData.Select(x => x.QS).Distinct();
            WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS = 17;
            WEEKLY_POS_NUMBER_OF_WEEKS = 17;

            this.DAILY_QA_AND_WEEKLY_QS = String.Format("{0} {1} Daily QA and Weekly QS", customerName, tab.SKU.SKUID);
            this.WEEKLY_QS_ORDER_QUANTITY = String.Format("{0} {1} Weekly QS, OUTS, and Order Quantity", customerName, tab.SKU.SKUID);
            this.WEEKLY_POS_HEADER = String.Format("{0} {1} Weekly POS Cases", customerName, tab.SKU.SKUID);
            this.CUSTOMERNUMBER = customerNumber;
            this.SKU = tab.SKU.SKUID;

            
        }
        #region properties
    
        private StackPanel _POS_COLLECTION_WAITING_STACKPANEL;        
        private List<daisybrand.forecaster.Presentation.UserControls.Promotions> _PROMOTION_USERCONTROLS;
        private List<daisybrand.forecaster.Presentation.UserControls.Percentage> _PERCENTAGE_USERCONTROLS;
        private List<daisybrand.forecaster.Presentation.UserControls.Volatility> _VOLATILITY_USERCONTROLS;
        private string _WEEKLY_POS_HEADER;
        private object _WEEKLY_POS_CASES_CONTENT;
        private List<daisybrand.forecaster.Presentation.UserControls.WeeklyPOSCases> _WEEKLY_POS_CASES_USERCONTROLS;
        private int _WEEKLY_POS_NUMBER_OF_WEEKS;
        private int _WEEKLY_POS_NUMBER_OF_DAYS;
       
        private DateTime[] _DAILY_QA_WEEKLY_QS_Y_Range;
        private IEnumerable<int> _DAILY_QA_WEEKLY_QS_X_Range;
        private int _DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS;
        private string _DAILY_QA_AND_WEEKLY_QS;
        private int _WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS;
        private int _VOLATILITY_NUMBER_OF_DAYS;
        private int _WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS;
        private int _DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS;        
        private int _MAX_WIDTH;
        private int _MAX_HEIGHT;
        private List<Presentation.UserControls.Distribution> _DISTRIBUTION_USERCONTROLS;
        private Dictionary<int, double> _QS_ZSCORES;
        private decimal _QS_MEDIAN;
        private double _QS_VOLATILITY;
        private int _VOLATILITY_NUMBER_OF_WEEKS;
        private int _LEFT;
        private int _TOP;
        private int _WIDTH;
        private int _HEIGHT;
        
        private double _QS_VARIANCE_S;
        private double _QS_VARIANCE_P;
        private double _QS_STANDARD_DEVIATION_S;
        private double _QS_STANDARD_DEVIATION_P;
        private double _QS_SUM;
        private double _QS_AVERAGE;
        
        private bool _IS_ORDERCOLLECTION_LOADED;
        private bool _IS_POSCOLLECTION_LOADING;
        private bool _IS_POSCOLLECTION_LOADED;
        private PosCollections _POSCOLLECTION;
        private string _WEEKLY_QS_ORDER_QUANTITY;
        private string _SKU;
        private string _CUSTOMERNUMBER;        
        private OrderCasesCollection _ORDERCOLLECTION;
        private List<daisybrand.forecaster.Presentation.UserControls.WeeklyQSOrderQuantity> _WEEKLY_QS_ORDER_QUANTITY_USERCONTROLS;
        private List<daisybrand.forecaster.Presentation.UserControls.DailyQAWeeklyQS> _DAILY_QA_WEEKLY_QS_USERCONTROLS;
        private DataCollection _DAILY_DATA_COLLECTION;
#endregion

        #region general
        public int MAX_HEIGHT
        {
            get
            {
                return _MAX_HEIGHT;
            }
            set
            {
                _MAX_HEIGHT = value;
                NotifyPropertyChanged();
            }
        }

        public int MAX_WIDTH
        {
            get
            {
                return _MAX_WIDTH;
            }
            set
            {
                _MAX_WIDTH = value;
                NotifyPropertyChanged();
            }
        }

        public int ORIG_HEIGHT { get; set; }
        public int HEIGHT
        {
            get
            {
                return _HEIGHT;
            }
            set
            {
                _HEIGHT = value;
                NotifyPropertyChanged();
            }
        }

        public int ORIG_WIDTH { get; set; }
        public int WIDTH
        {
            get
            {
                return _WIDTH;
            }
            set
            {
                _WIDTH = value;
                NotifyPropertyChanged();
            }
        }

        public int TOP
        {
            get
            {
                return _TOP;
            }
            set
            {
                _TOP = value;
                NotifyPropertyChanged();
            }
        }

        public int LEFT
        {
            get
            {
                return _LEFT;
            }
            set
            {
                _LEFT = value;
                NotifyPropertyChanged();
            }
        }

        public string CUSTOMERNUMBER
        {
            get
            {
                return _CUSTOMERNUMBER;
            }
            set
            {
                _CUSTOMERNUMBER = value;
                NotifyPropertyChanged();
            }
        }
        public string SKU
        {
            get
            {
                return _SKU;
            }
            set
            {
                _SKU = value;
                NotifyPropertyChanged();
            }
        }

        public Window WINDOW { get; set; }
        #endregion

        #region weekly pos
        public PosCollections POSCOLLECTION
        {
            get
            {
                return _POSCOLLECTION;
            }
            set
            {
                _POSCOLLECTION = value;
                //_UpdateWeeklyPosCases();
                NotifyPropertyChanged();
            }
        }

        public bool IS_POSCOLLECTION_LOADED
        {
            get
            {
                return _IS_POSCOLLECTION_LOADED;
            }
            set
            {
                _IS_POSCOLLECTION_LOADED = value;

                if (value)
                {
                    NotifyPropertyChanged("WEEKLY_POS_CASES_CONTENT");
                    //WEEKLY_POS_CASES_CONTENT = new Presentation.UserControls.WeeklyPOSCases(TAB);
                }
                NotifyPropertyChanged();
            }
        }

        private void _UpdateWeeklyPosCases()
        {
            if (this.WEEKLY_POS_CASES_CONTENT != null && this.WEEKLY_POS_CASES_CONTENT.GetType().Equals(typeof(Presentation.UserControls.WeeklyPOSCases)))
                ((Presentation.UserControls.WeeklyPOSCases)this.WEEKLY_POS_CASES_CONTENT).Update(this.WEEKLY_POS_NUMBER_OF_DAYS);
            //foreach (var u in WEEKLY_POS_CASES_USERCONTROLS)
            //    u.Update(this.WEEKLY_POS_NUMBER_OF_DAYS);
        }

        public bool IS_POSCOLLECTION_LOADING
        {
            get
            {
                return _IS_POSCOLLECTION_LOADING;
            }
            set
            {
                _IS_POSCOLLECTION_LOADING = value;
                if (value)
                {
                    _SetWEEKLY_POS_CASES_CONTENT_WaitingText(true);
                }

                NotifyPropertyChanged();
            }
        }

        public StackPanel POS_COLLECTION_WAITING_STACKPANEL
        {
            get
            {
                return _POS_COLLECTION_WAITING_STACKPANEL;
            }
            set
            {
                _POS_COLLECTION_WAITING_STACKPANEL = value;
                NotifyPropertyChanged();
            }
        }

        public void SetWEEKLY_POS_CASES_CONTENT_ErrorText()
        {
            WEEKLY_POS_HAS_ERROR = true;
            _SetWEEKLY_POS_CASES_CONTENT_WaitingText(false, Properties.Settings.Default.ERROR);
        }


        private void _SetWEEKLY_POS_CASES_CONTENT_WaitingText(bool addImage, string text = "Long running query. Please wait ...")
        {
            if (POS_COLLECTION_WAITING_STACKPANEL != null)
                POS_COLLECTION_WAITING_STACKPANEL.Children.RemoveAt(0);
            POS_COLLECTION_WAITING_STACKPANEL = null;

            POS_COLLECTION_WAITING_STACKPANEL = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            if (addImage)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("pack://application:,,,/Presentation/Resources/Images/waiting.gif");
                image.EndInit();
                Image img = new Image() { Width = 32, Height = 64 };
                ImageBehavior.SetAnimatedSource(img, image);
                ImageBehavior.SetRepeatBehavior(img, System.Windows.Media.Animation.RepeatBehavior.Forever);
                POS_COLLECTION_WAITING_STACKPANEL.Children.Add(img); //(Image)Application.Current.Resources["waitingImage"]);
            }

            POS_COLLECTION_WAITING_STACKPANEL.Children.Add(new TextBlock { VerticalAlignment = VerticalAlignment.Center, Text = text });
            if (!addImage)
            {
                var refresh = new Button()
                {
                    Content = "Refresh",
                };
                refresh.Click += (s, e) =>
                {
                    TAB.myGraphViewModel.POS_COLLECTION_WAITING_STACKPANEL = null;
                    TAB.myGraphViewModel.IS_POSCOLLECTION_LOADED = false;

                    PosCollections.GetAsync(TAB);
                };
                POS_COLLECTION_WAITING_STACKPANEL.Children.Add(refresh);
            }
            
            WEEKLY_POS_CASES_CONTENT = POS_COLLECTION_WAITING_STACKPANEL;
        }

        private bool WEEKLY_POS_HAS_ERROR { get; set; }
        public string WEEKLY_POS_HEADER
        {
            get
            {
                return _WEEKLY_POS_HEADER;
            }
            set
            {
                _WEEKLY_POS_HEADER = value;
            }
        }
        
        public int WEEKLY_POS_NUMBER_OF_DAYS
        {
            get
            {
                return _WEEKLY_POS_NUMBER_OF_DAYS;
            }
            set
            {
                _WEEKLY_POS_NUMBER_OF_DAYS = value;
                NotifyPropertyChanged();
            }
        }

        public int WEEKLY_POS_NUMBER_OF_WEEKS
        {
            get
            {
                return _WEEKLY_POS_NUMBER_OF_WEEKS;
            }
            set
            {
                _WEEKLY_POS_NUMBER_OF_WEEKS = value;
                WEEKLY_POS_NUMBER_OF_DAYS = value * 7;
                 //_UpdateWeeklyPosCases();
                NotifyPropertyChanged();
            }
        }

        public Object WEEKLY_POS_CASES_CONTENT
        {
            get
            {
                if (IS_POSCOLLECTION_LOADED && !WEEKLY_POS_HAS_ERROR)
                    _WEEKLY_POS_CASES_CONTENT = new forecaster.Presentation.UserControls.WeeklyPOSCases(this.TAB);
                return _WEEKLY_POS_CASES_CONTENT;
            }
            private set
            {
                _WEEKLY_POS_CASES_CONTENT = value;
                NotifyPropertyChanged();
            }
        }

        public void DisposeWEEKLY_POS_CASES_CONTENT()
        {
            _WEEKLY_POS_CASES_CONTENT = null;    
        }


        #endregion

        #region daily qa weekly qs

        public object DAILY_QA_AND_WEEKLY_QS_CONTENT
        {
            get
            {
                return _DAILY_QA_AND_WEEKLY_QS_CONTENT;
            }
            set
            {
                if (_DAILY_QA_AND_WEEKLY_QS_CONTENT != null) return;
                _DAILY_QA_AND_WEEKLY_QS_CONTENT = value;
                NotifyPropertyChanged();
            }
        }

        public object DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT
        {
            get
            {
                return _DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT;
            }
            set
            {
                _DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT = value;
                NotifyPropertyChanged();
            }
        }

        public bool Is_DAILY_QA_AND_WEEKLY_QS_CONTENT_Null()
        {
            return this._DAILY_QA_AND_WEEKLY_QS_CONTENT == null;
        }
        
        public void Set_DAILY_QA_AND_WEEKLY_QS_CONTENT(string text)
        {
            _DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT = _DAILY_QA_AND_WEEKLY_QS_CONTENT = null;
            StackPanel s = new StackPanel();
            s.Children.Add(new TextBlock() { Text = text });
            DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT = DAILY_QA_AND_WEEKLY_QS_CONTENT = s;
        }

        public string DAILY_QA_AND_WEEKLY_QS
        {
            get
            {
                return _DAILY_QA_AND_WEEKLY_QS;
            }
            set
            {
                if (_DAILY_QA_AND_WEEKLY_QS == value) return;
                _DAILY_QA_AND_WEEKLY_QS = value;
                NotifyPropertyChanged();
            }
        }
        
        public int DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS
        {
            get
            {
                return _DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS;
            }
            set
            {
                _DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS = value;
                NotifyPropertyChanged();
            }
        }


        public int DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS
        {
            get
            {
                return _DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS;
            }
            set
            {
                _DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS = value;
                DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS = value * 7;
                _UpdateDailyQAWeeklyQS();
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<int> DAILY_QA_WEEKLY_QS_X_Range
        {
            get
            {
                return _DAILY_QA_WEEKLY_QS_X_Range;
            }
            set
            {
                _DAILY_QA_WEEKLY_QS_X_Range = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime[] DAILY_QA_WEEKLY_QS_Y_Range
        {
            get
            {
                return _DAILY_QA_WEEKLY_QS_Y_Range;
            }
            set
            {
                _DAILY_QA_WEEKLY_QS_Y_Range = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region weekly qs order quantity
        public object WEEKLY_QS_ORDER_QUANTITY_CONTENT
        {
            get
            {
                return _WEEKLY_QS_ORDER_QUANTITY_CONTENT;
            }
            set
            {
                if (_WEEKLY_QS_ORDER_QUANTITY_CONTENT != null) return;
                _WEEKLY_QS_ORDER_QUANTITY_CONTENT = value;
                NotifyPropertyChanged();
            }
        }

 
        public object WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT
        {
            get
            {
                return _WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT;
            }
            set
            {
                //if (_WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT != null) return;
                _WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT = value;
                NotifyPropertyChanged();
            }
        }


        public string WEEKLY_QS_ORDER_QUANTITY
        {
            get
            {
                return _WEEKLY_QS_ORDER_QUANTITY;
            }
            set
            {
                _WEEKLY_QS_ORDER_QUANTITY = value;
                NotifyPropertyChanged();
            }
        }

        public void Set_WEEKLY_QS_ORDER_QUANTITY_CONTENT(string text)
        {
            _WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT = _WEEKLY_QS_ORDER_QUANTITY_CONTENT = null;
            StackPanel s = new StackPanel();
            s.Children.Add(new TextBlock() { Text = text });
            WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT = WEEKLY_QS_ORDER_QUANTITY_CONTENT = s;
        }

        public int WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS
        {
            get
            {
                return _WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS;
            }
            set
            {
                _WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS = value;
                NotifyPropertyChanged();
            }
        } 


        public int WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS
        {
            get
            {
                return _WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS;
            }
            set
            {
                _WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS = value;
                WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS = value * 7;
                _UpdateWeeklyQSOrderQuantity();
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region volatility

        public object VOLATILITY_CONTENT
        {
            get
            {
                return _VOLATILITY_CONTENT;
            }
            set
            {
                if (_VOLATILITY_CONTENT == value) return;
                _VOLATILITY_CONTENT = value;
                NotifyPropertyChanged();
            }
        }

        public int VOLATILITY_NUMBER_OF_DAYS
        {
            get
            {
                return _VOLATILITY_NUMBER_OF_DAYS;
            }
            set
            {
                _VOLATILITY_NUMBER_OF_DAYS = value;
            }  
          
        }

        public int VOLATILITY_NUMBER_OF_WEEKS
        {
            get
            {
                return _VOLATILITY_NUMBER_OF_WEEKS;
            }
            set
            {
                if (_VOLATILITY_NUMBER_OF_WEEKS == value) return;
                _VOLATILITY_NUMBER_OF_WEEKS = value;
                VOLATILITY_NUMBER_OF_DAYS = value * 7;
                if (TAB.myWeeklyData != null)
                    SetQSStatisticalData();                
                NotifyPropertyChanged();
            }
        }

        public Dictionary<int, double> QS_ZSCORES
        {
            get
            {
                return _QS_ZSCORES;
            }
            set
            {
                _QS_ZSCORES = value;
                if (this.DISTRIBUTION_CONTENT != null)
                    ((Presentation.UserControls.Distribution)this.DISTRIBUTION_CONTENT).Update();
                NotifyPropertyChanged();
            }
        }

        public double QS_VOLATILITY
        {
            get
            {
                return _QS_VOLATILITY;
            }
            set
            {
                _QS_VOLATILITY = value;
                NotifyPropertyChanged();
            }
        }
        
        public double QS_AVERAGE
        {
            get
            {
                return Math.Round(_QS_AVERAGE, 2);
            }
            private set
            {
                _QS_AVERAGE = value;
                NotifyPropertyChanged();
            }
        }

        public decimal QS_MEDIAN
        {
            get
            {
                return _QS_MEDIAN;
            }
            set
            {
                _QS_MEDIAN = value;
                NotifyPropertyChanged();
            }
        }

        public double QS_SUM
        {
            get
            {
                return _QS_SUM;
            }
            private set
            {
                _QS_SUM = value;
                NotifyPropertyChanged();
            }
        }


        public double QS_VARIANCE_P
        {
            get
            {
                return Math.Round(_QS_VARIANCE_P, 2);
            }
            private set
            {
                _QS_VARIANCE_P = value;
                QS_STANDARD_DEVIATION_P = value.GetStdDev();
                NotifyPropertyChanged();
            }
        }

        public double QS_VARIANCE_S
        {
            get
            {
                return Math.Round(_QS_VARIANCE_S, 2);
            }
            set
            {
                _QS_VARIANCE_S = value;
                QS_STANDARD_DEVIATION_S = value.GetStdDev();
                NotifyPropertyChanged();
            }
        }

        public double QS_STANDARD_DEVIATION_P
        {
            get
            {
                return Math.Round(_QS_STANDARD_DEVIATION_P, 2);
            }
            set
            {
                _QS_STANDARD_DEVIATION_P = value;
                NotifyPropertyChanged();
            }
        }

        public double QS_STANDARD_DEVIATION_S
        {
            get
            {
                return Math.Round(_QS_STANDARD_DEVIATION_S, 2);
            }
            set
            {
                _QS_STANDARD_DEVIATION_S = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region collections
        public bool Is_DAILY_DATA_COLLECTION_Loading
        {
            get
            {
                return _Is_DAILY_DATA_COLLECTION_Loading;
            }
            set
            {
                if (_Is_DAILY_DATA_COLLECTION_Loading == value) return;
                _Is_DAILY_DATA_COLLECTION_Loading = value;
                NotifyPropertyChanged();
            }
        }

        public bool Is_DAILY_DATA_COLLECTION_Loaded
        {
            get
            {
                return _Is_DAILY_DATA_COLLECTION_Loaded;
            }
            set
            {
                if (_Is_DAILY_DATA_COLLECTION_Loaded == value) return;
                _Is_DAILY_DATA_COLLECTION_Loaded = value;
                NotifyPropertyChanged();
            }
        }

        
        public OrderCasesCollection ORDERCOLLECTION
        {
            get
            {
                return _ORDERCOLLECTION;
            }
            set
            {
                _ORDERCOLLECTION = value;                
                NotifyPropertyChanged();
            }
        }

        public bool IS_ORDERCOLLECTION_LOADING
        {
            get
            {
                return _IS_ORDERCOLLECTION_LOADING;
            }
            set
            {
                if (_IS_ORDERCOLLECTION_LOADING == value) return;
                _IS_ORDERCOLLECTION_LOADING = value;
                NotifyPropertyChanged();
            }
        }

        public bool IS_ORDERCOLLECTION_LOADED
        {
            get
            {
                return _IS_ORDERCOLLECTION_LOADED;
            }
            set
            {
                _IS_ORDERCOLLECTION_LOADED = value;
                if (value && this.WEEKLY_QS_ORDER_QUANTITY_CONTENT != null)
                    ((Presentation.UserControls.WeeklyQSOrderQuantity)this.WEEKLY_QS_ORDER_QUANTITY_CONTENT).Update(this.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS);
                NotifyPropertyChanged();
            }
        }


    

       
        #endregion

        #region distribution

        public object DISTRIBUTION_CONTENT
        {
            get
            {
                return _DISTRIBUTION_CONTENT;
            }
            set
            {
                if (_DISTRIBUTION_CONTENT == value) return;
                _DISTRIBUTION_CONTENT = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region percentages

        public object PERCENTAGE_CONTENT
        {
            get
            {
                return _PERCENTAGE_CONTENT;
            }
            set
            {
                if (_PERCENTAGE_CONTENT == value) return;
                _PERCENTAGE_CONTENT = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region promotions

        public object PROMOTION_CONTENT
        {
            get
            {
                return _PROMOTION_CONTENT;
            }
            set
            {
                if (_PROMOTION_CONTENT == value) return;
                _PROMOTION_CONTENT = value;
                NotifyPropertyChanged();
            }
        }

        #endregion




        private void _UpdateDailyQAWeeklyQS()
        {
            if (_DAILY_QA_AND_WEEKLY_QS_CONTENT != null && _DAILY_QA_AND_WEEKLY_QS_CONTENT.GetType().Equals(typeof(Presentation.UserControls.DailyQAWeeklyQS)))
                ((Presentation.UserControls.DailyQAWeeklyQS)_DAILY_QA_AND_WEEKLY_QS_CONTENT).Update(this.DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS);
            if (_DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT != null && _DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT.GetType().Equals(typeof(Presentation.UserControls.DailyQAWeeklyQS)))
                ((Presentation.UserControls.DailyQAWeeklyQS)_DAILY_QA_AND_WEEKLY_QS_ALL_CONTENT).Update(this.DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS);
            //foreach (var u in DAILY_QA_WEEKLY_QS_USERCONTROLS)
            //    u.Update(this.DAILY_QA_WEEKLY_QS_NUMBER_OF_DAYS);
        }

        private void _UpdateWeeklyQSOrderQuantity()
        {
            if (_WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT != null && _WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT.GetType().Equals(typeof(Presentation.UserControls.WeeklyQSOrderQuantity)))
                ((Presentation.UserControls.WeeklyQSOrderQuantity)_WEEKLY_QS_ORDER_QUANTITY_ALL_CONTENT).Update(this.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS);
            if (_WEEKLY_QS_ORDER_QUANTITY_CONTENT != null && _WEEKLY_QS_ORDER_QUANTITY_CONTENT.GetType().Equals(typeof(Presentation.UserControls.WeeklyQSOrderQuantity)))
                ((Presentation.UserControls.WeeklyQSOrderQuantity)_WEEKLY_QS_ORDER_QUANTITY_CONTENT).Update(this.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS);
            //foreach (var u in WEEKLY_QS_ORDER_QUANTITY_USERCONTROLS)
            //    u.Update(this.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_DAYS);
        }

        public void SetQSStatisticalData()
        {
            var data = TAB.myWeeklyData;
            if (data == null) return;
            var list1 = data.Where(x => x.REPORT_AS_OF_DATE <= data.LASTDAYOFLASTWEEKWITHREALDATA)
                .Where(x => !x.HAS_HOLIDAY && !x.HAS_PERFORMANCES)
                .OrderByDescending(x => x.REPORT_AS_OF_DATE).Take(this.VOLATILITY_NUMBER_OF_WEEKS);
            var QSData = list1.Select(x => x.QS);
            if (QSData.Count() == 0) return;
            QS_AVERAGE = QSData.Average();
            QS_MEDIAN = QSData.Median();
            QS_SUM = QSData.Sum();
            QS_VARIANCE_P = QSData.GetVarianceP();
            QS_VARIANCE_S = QSData.GetVarianceS();
            QS_VOLATILITY = QS_STANDARD_DEVIATION_S.GetVolatility(QS_AVERAGE);
            QS_ZSCORES = list1.SelectMany(x=>x.DAILY_DATA).Select(x=>x.QS).ZScore();      
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
