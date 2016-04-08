using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using daisybrand.forecaster.Controlers.Collections;
using daisybrand.forecaster.Controlers.Fields;
namespace daisybrand.forecaster.Controlers.Objects
{
    public class DailyData : IDailyData, INotifyPropertyChanged
    {
        public event EventHandler RefreshDataContext;

        public DailyData(
            bool canRefresh, int WEEK_NUMBER, int YEAR,  string SHIP_TO_TP, string ITEM_UPC)
            : base()
        {
            _CANEXECUTE = canRefresh;
            _WEEK_NUMBER = WEEK_NUMBER;
            _SHIP_TO_TP = SHIP_TO_TP;
            _ITEM_UPC = ITEM_UPC;
            _YEAR = YEAR;
            _BASE_LINE = new BaseLine((IDailyData)this);
            _BASE_INDEX = new BaseIndex((IDailyData)this);

        }

        #region IQuantity Members
        private DaysOfSupplies _DAYS_OF_SUPPLIES;
        private int _WTD_QS;
        private BaseIndex _BASE_INDEX;
        private BaseLine _BASE_LINE;
        private string _QO_COMMENT_VALUE;
        private int _QONumberOfNoneZeros;
        private bool _IS_FUTURE;
        private ICellComment _ORDER_DELIVERY_COMMENT;
        private decimal _ORDER_DELIVERY;
        private string _HOLIDAY_TOOLTIP;
        private bool _HAS_PERFORMANCES;
        private bool _HAS_HOLIDAY;
        private IEnumerable<DateTime> _ALL_DAYS_IN_WEEK;
        private HolidayCollection _HOLIDAYS;
        private int _QP;
        //private IEnumerable<string> _ALL_WEEK_ID;
        private IEnumerable<DateTime> _ALL_REPORT_AS_OF_DATE;
        private DateTime _FIRST_DAY_OF_WEEK;
        private int _WEEK_NUMBER;
        private int _YEAR;
        private ICellComment _LY_COMMENT;
        private IFsi _FSI;
        private ICellComment _QW_COMMENT;
        private ICellComment _QO_COMMENT;
        private ICellComment _QS_COMMENT;
        private ICellComment _QA_COMMENT;
        private IEnumerable<IDailyData> _SUB_QUANTITIES;
        
        private DateTime _DATE_TIME;
        private PerformanceCollection _PERFORMANCES;
  
        private System.Windows.Input.ICommand _REFRESH_COMMAND;
        private bool _CANEXECUTE;
        
        private IComment _COMMENT;        
        private ForecastCollection _FORECASTS;
        private IQcQd _QC;
        private IQcQd _QD;
        private int _LYQW;
        private int _LYQS;
        //private int _LAST_FORECAST;
        private DateTime _REPORT_AS_OF_DATE;
        private int _QW;
        private int _QO;
        private int _QS;
        private int _QA;
        
        private string _ITEM_UPC;
        
        private string _UOM;
        private string _SHIP_TO_TP;
        //private Guid _DOCUMENTOBJECTID;

        uint IDailyData.INDEX { get; set; }
        
        IEnumerable<IDailyData> IDailyData.DAILY_DATA
        {
            get
            {
                return _SUB_QUANTITIES;
            }
            set
            {
                _SUB_QUANTITIES = value;
                foreach (var i in value)
                {
                    if (!i.IS_FUTURE)
                    {
                        var previousDays = value.Where(x => x.REPORT_AS_OF_DATE <= i.REPORT_AS_OF_DATE);
                        i.WTD_QS = previousDays.Sum(x => x.QS);
                    }
                }
            }
        }

        public DateTime DATE_TIME
        {
            get
            {
                return _DATE_TIME;
            }
            set
            {
                _DATE_TIME = value;
                NotifyPropertyChanged();
            }
        }      

        string IDailyData.SHIP_TO_TP
        {
            get
            {
                return _SHIP_TO_TP;
            }
            set
            {
                if (_SHIP_TO_TP == value) return;
                _SHIP_TO_TP = value;
                NotifyPropertyChanged();
            }
        }

        int IDailyData.QP
        {
            get
            {
                return _QP;
            }
            set
            {
                if (_QP == value) return;
                _QP = value;
                NotifyPropertyChanged();
            }
        }

        int IDailyData.QA
        {
            get
            {
                return _QA;
            }
            set
            {
                if (_QA == value) return;
                _QA = value;                
                NotifyPropertyChanged();
            }
        }

        int IDailyData.QS
        {
            get
            {
                return _QS;
            }
            set
            {
                if (_QS == value) return;
                _QS = value;
                NotifyPropertyChanged();
                ((IDailyData)this).BASE_INDEX.NotifyPropertyChanged(null);
            }
        }

        int IDailyData.WTD_QS
        {
            get
            {
                return _WTD_QS;
            }
            set
            {
                _WTD_QS = value;
            }
        }
        int IDailyData.QO
        {
            get
            {
                return _QO;
            }
            set
            {
                if (_QO == value) return;
                _QO = value;
                NotifyPropertyChanged();
            }
        }

        int IDailyData.QONumberOfNoneZeros
        {
            get
            {
                return _QONumberOfNoneZeros;
            }
            set
            {
                _QONumberOfNoneZeros = value;
                NotifyPropertyChanged();
            }
        }
        int IDailyData.QW
        {
            get
            {
                return _QW;
            }
            set
            {
                if (_QW == value) return;
                _QW = value;
                NotifyPropertyChanged();
            }
        }

        int IDailyData.LYQS
        {
            get
            {
                return _LYQS;
            }
            set
            {
                if (_LYQS == value) ;
                _LYQS = value;
                NotifyPropertyChanged();
            }
        }

        int IDailyData.LYQW
        {
            get
            {
                return _LYQW;
            }
            set
            {
                if (_LYQW == value) return;
                _LYQW = value;
                NotifyPropertyChanged();
            }
        }

        string IDailyData.UOM
        {
            get
            {
                return _UOM;
            }
            set
            {
                if (_UOM == value) return;
                _UOM = value;
                NotifyPropertyChanged();
            }
        }

        DateTime IDailyData.REPORT_AS_OF_DATE
        {
            get
            {
                return _REPORT_AS_OF_DATE;
            }
            set
            {
                // if (_REPORT_AS_OF_DATE == value) return;
                _REPORT_AS_OF_DATE = value;
                this.DATE_TIME = value;
                NotifyPropertyChanged();
            }
        }

        IEnumerable<DateTime> IDailyData.ALL_REPORT_AS_OF_DATE
        {
            get
            {
                return _ALL_REPORT_AS_OF_DATE;
            }
            set
            {
                _ALL_REPORT_AS_OF_DATE = value;
            }
        }

        IEnumerable<DateTime> IDailyData.ALL_DAYS_IN_WEEK
        {
            get
            {
                return _ALL_DAYS_IN_WEEK;
            }
            set
            {
                _ALL_DAYS_IN_WEEK = value;
            }
        }

        DateTime IDailyData.LAST_YEAR_DATE
        {
            get
            {
                return this._REPORT_AS_OF_DATE.LastYearDate();
            }
        }

        DateTime IDailyData.FIRST_DAY_OF_WEEK
        {
            get
            {
                return _FIRST_DAY_OF_WEEK;
            }
            set
            {
                _FIRST_DAY_OF_WEEK = value;
                _ALL_DAYS_IN_WEEK = value.GetAllDaysInWeek();
            }
        }
        string IDailyData.DAY_OF_WEEK
        {
            get { return this._REPORT_AS_OF_DATE.DayOfWeek.ToString(); }
        }

        int IDailyData.WEEK_NUMBER
        {
            get
            {
                //ship_to_tp give me the dc number to get the starting day of the week
                //return this._REPORT_AS_OF_DATE.WeekNumber(_Date.DayOfWeek(this._SHIP_TO_TP));
                return _WEEK_NUMBER;
            }
            set
            {
                _WEEK_NUMBER = value;
            }
        }

        string IDailyData.WEEK_ID
        {
            get
            {
                return String.Format("{0}{1}{2}{3}", ((IDailyData)this).WEEK_NUMBER, ((IDailyData)this).YEAR, ((IDailyData)this).SHIP_TO_TP, ((IDailyData)this).ITEM_UPC);
            }
        }


        string IDailyData.ITEM_UPC
        {
            get
            {
                return _ITEM_UPC;
            }
            set
            {
                if (_ITEM_UPC == value) return;
                _ITEM_UPC = value;
                NotifyPropertyChanged();
            }
        }

        int IDailyData.YEAR
        {
            get
            {
                return _YEAR;
            }
            set
            {
                _YEAR = value;
            }

        }

        IComment IDailyData.COMMENT
        {
            get
            {
                return _COMMENT;
            }
            set
            {
                if (_COMMENT == value) return;
                _COMMENT = value;
                NotifyPropertyChanged();
            }
        }

        decimal IDailyData.ORDER_DELIVERY
        {
            get
            {
                return Math.Round(_ORDER_DELIVERY, 0);
            }
            set
            {
                _ORDER_DELIVERY = value;
                NotifyPropertyChanged();
            }
        }

        //int IQuantity.LAST_FORECAST
        //{
        //    get
        //    {
        //        return _LAST_FORECAST;
        //    }
        //    set
        //    {
        //        if (_LAST_FORECAST == value) return;
        //        _LAST_FORECAST = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        ForecastCollection IDailyData.FORECASTS
        {
            get
            {
                return _FORECASTS;
            }
            set
            {
                _FORECASTS = value;
                //((IQuantity)this).FORECAST_TOOLTIP = _GetForecastTooltip();
                //if (this._FORECASTS != null) ((IQuantity)this).LAST_FORECAST = value.OrderByDescending(x => x.DATE_ENTERED).Select(x => x.VALUE).FirstOrDefault();
                NotifyPropertyChanged();
            }
        }

        //string IQuantity.FORECAST_TOOLTIP
        //{
        //    get
        //    {
        //        return _FORECAST_TOOLTIP;
        //    }
        //    set
        //    {
        //        if (_FORECAST_TOOLTIP == value) return;
        //        _FORECAST_TOOLTIP = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        IQcQd IDailyData.QC
        {
            get
            {
                return _QC;
            }
            set
            {
                if (_QC == value) return;
                _QC = value;
                NotifyPropertyChanged();
            }
        }

        IQcQd IDailyData.QD
        {
            get
            {
                return _QD;
            }
            set
            {
                if (_QD == value) return;
                _QD = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        
        #region refresh command


        System.Windows.Input.ICommand IDailyData.REFRESH_COMMAND
        {
            get
            {
                return _REFRESH_COMMAND ?? (new Commands.RefreshHandler(() => ((IDailyData)this).REFRESH_ACTION(), _CANEXECUTE));
            }
        }

        bool IDailyData.CANEXECUTE
        {
            get
            {
                return _CANEXECUTE;
            }
            set
            {
                if (_CANEXECUTE == false) return;
                _CANEXECUTE = value;
                NotifyPropertyChanged();
            }
        }

        void IDailyData.REFRESH_ACTION()
        {
            IDailyData q = (IDailyData)this;
            //_UpdateDailyRecord(q);
            if (RefreshDataContext != null)
                RefreshDataContext(this, EventArgs.Empty);
        }

        #endregion

        
        #region IQuantity Members

        HolidayCollection IDailyData.HOLIDAYS
        {
            get
            {
                return _HOLIDAYS;
            }
            set
            {               
                _HOLIDAYS = value;
                if (value == null) return;
                value.TOOLTIP = _GetHolidayTooltip();
                value.HAS_HOLIDAYS = value.Count() > 0;  
                NotifyPropertyChanged();
                ((IDailyData)this).HAS_HOLIDAY = value.Count() > 0;                
                ((IDailyData)this).HOLIDAY_TOOLTIP = _GetHolidayTooltip();                
            }
        }

        void IDailyData.HolidayAdding(IHoldiay item)
        {
            if (_HOLIDAYS == null) _HOLIDAYS = new HolidayCollection();
            _HOLIDAYS.Add(item);
            _HOLIDAYS.TOOLTIP = _GetHolidayTooltip();
            _HOLIDAYS.HAS_HOLIDAYS = _HOLIDAYS.Count() > 0;
            ((IDailyData)this).HAS_HOLIDAY = _HOLIDAYS.Count() > 0;
            ((IDailyData)this).HOLIDAY_TOOLTIP = _GetHolidayTooltip(); 
            NotifyPropertyChanged(null);
        }

        void IDailyData.HolidayRemoving(IHoldiay item)
        {
            _HOLIDAYS.Remove(item);
            _HOLIDAYS.TOOLTIP = _GetHolidayTooltip();
            _HOLIDAYS.HAS_HOLIDAYS = _HOLIDAYS.Count() > 0;
            ((IDailyData)this).HAS_HOLIDAY = _HOLIDAYS.Count() > 0;
            ((IDailyData)this).HOLIDAY_TOOLTIP = _GetHolidayTooltip(); 
            NotifyPropertyChanged(null);
        }

        bool IDailyData.HAS_HOLIDAY
        {
            get
            {
                return _HAS_HOLIDAY;
            }
            set
            {
                _HAS_HOLIDAY = value;                
                NotifyPropertyChanged();
            }
        }

        string IDailyData.HOLIDAY_TOOLTIP
        {
            get
            {
                return _HOLIDAY_TOOLTIP;
            }
            set
            {
                _HOLIDAY_TOOLTIP = value;
                NotifyPropertyChanged();
            }
        }

        PerformanceCollection IDailyData.PERFORMANCES
        {
            get
            {
                return _PERFORMANCES;
            }
            set
            {
                _PERFORMANCES = value;
                //((IDailyData)this).HAS_PERFORMANCES = value != null && value.Count() > 0 && value.Any(x => x.IS_INCLUDED == true);
                NotifyPropertyChanged("HAS_PERFORMANCES");
                NotifyPropertyChanged();
            }
        }

        bool IDailyData.HAS_PERFORMANCES
        {
            get
            {
                return ((IDailyData)this).PERFORMANCES != null && ((IDailyData)this).PERFORMANCES.Count() > 0 && ((IDailyData)this).PERFORMANCES.Any(x => x.IS_INCLUDED == true);
            }           
        }

        bool IDailyData.IS_FUTURE
        {
            get
            {
                return _IS_FUTURE;
            }
            set
            {
                _IS_FUTURE = value;
                NotifyPropertyChanged();
            }
        }

        void IDailyData.AddPerformance(IPerformance item)
        {
            _PERFORMANCES.Add(item);
            NotifyPropertyChanged("HAS_PERFORMANCES");
            //((IDailyData)this).UPDATE_PERFORMANCES();            
        }

        void IDailyData.DeletePerformance(IPerformance perf)
        {
            ((IDailyData)this).PERFORMANCES.Remove(perf);
            NotifyPropertyChanged("HAS_PERFORMANCES");
        }

        ///// <summary>
        ///// WHETHER OR NOT THERE ARE PERFORMANCES
        ///// </summary>
        //void IDailyData.UPDATE_PERFORMANCES()
        //{
        //    ((IDailyData)this).HAS_PERFORMANCES = ((IDailyData)this).PERFORMANCES != null && ((IDailyData)this).PERFORMANCES.Count() > 0 && ((IDailyData)this).PERFORMANCES.Any(x => x.IS_INCLUDED == true);
        //}

        void IDailyData.OnPropertyChanged(string name)
        {
            NotifyPropertyChanged(name);
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


        #region IDailyData Members


        ICellComment IDailyData.QA_COMMENT
        {
            get
            {
                return _QA_COMMENT;
            }
            set
            {
                if (_QA_COMMENT == value) return;
                _QA_COMMENT = value;
                NotifyPropertyChanged();
            }
        }

        ICellComment IDailyData.QS_COMMENT
        {
            get
            {
                return _QS_COMMENT;
            }
            set
            {
                if (_QS_COMMENT == value) return;
                _QS_COMMENT = value;
                NotifyPropertyChanged();
            }
        }

        ICellComment IDailyData.QO_COMMENT
        {
            get
            {
                return _QO_COMMENT;
            }
            set
            {
                if (_QO_COMMENT == value) return;
                _QO_COMMENT = value;
                NotifyPropertyChanged();
            }
        }


        ICellComment IDailyData.QW_COMMENT
        {
            get
            {
                return _QW_COMMENT;
            }
            set
            {
                if (_QW_COMMENT == value) return;
                _QW_COMMENT = value;
                NotifyPropertyChanged();
            }
        }

        ICellComment IDailyData.LY_COMMENT
        {
            get
            {
                return _LY_COMMENT;
            }
            set
            {
                if (_LY_COMMENT == value) return;
                _LY_COMMENT = value;
                NotifyPropertyChanged();
            }
        }

        ICellComment IDailyData.ORDER_DELIVERY_COMMENT
        {
            get
            {
                return _ORDER_DELIVERY_COMMENT;
            }
            set
            {
                if (_ORDER_DELIVERY_COMMENT == value) return;
                _ORDER_DELIVERY_COMMENT = value;
                NotifyPropertyChanged();
            }
        }
        IFsi IDailyData.FSI
        {
            get
            {
                return _FSI;
            }
            set
            {
                if (_FSI == value) return;
                _FSI = value;
                NotifyPropertyChanged();
            }
        }

        BaseLine IDailyData.BASE_LINE
        {
            get
            {
                return _BASE_LINE;
            }
            set
            {
                _BASE_LINE = value;                
                NotifyPropertyChanged();
                value.NotifyPropertyChanged(null);
            }
        }

        BaseIndex IDailyData.BASE_INDEX
        {
            get
            {
                return _BASE_INDEX;
            }
            set
            {
                _BASE_INDEX = value;
                NotifyPropertyChanged();
                value.NotifyPropertyChanged(null);
            }
        }

        public DaysOfSupplies DAYS_OF_SUPPLIES
        {
            get
            {
                return _DAYS_OF_SUPPLIES;
            }
            set
            {
                _DAYS_OF_SUPPLIES = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        string _GetHolidayTooltip()
        {
            var sb = new StringBuilder();
            foreach (var h in ((IDailyData)this).HOLIDAYS)
                sb.Append(String.Format("{0} - {1}-{2}\r", h.DESCRIPTION, h.START.ToShortDateString(), h.END.ToShortDateString()));

            return sb.ToString() == string.Empty ? ((IDailyData)this).DAY_OF_WEEK : sb.ToString();
        }


    
    }
}
