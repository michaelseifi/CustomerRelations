using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daisybrand.forecaster.Helpers;
using daisybrand.forecaster.Extensions;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Controlers.Enums;
using daisybrand.forecaster.Controlers.Objects;
using datastores = CRDataStore;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CRDataStore;

namespace daisybrand.forecaster.Controlers.Collections
{
    public class DataCollection:
        ObservableCollection<IDailyData>, INotifyPropertyChanged, IDisposable
    {
        //public event EventHandler RefreshDataContext;
        public DataCollection(){}
        public DataCollection(ICustomer customer,  ITab tab, int numberOfPeriods, int view)
        {
            CUSTOMER = customer;
            TAB = tab;
            SKUID = tab.SKU;
            Number_Of_Years = numberOfPeriods;
            GetDaily(customer.ACCOUNTNUM, tab, numberOfPeriods, view);
        }
        public DataCollection(List<IDailyData> list)
            : base(list)
        {
            
        }
        public DataCollection(IEnumerable<IDailyData> collection)
            : base(collection)
        {
            
        }        

        public void AddDays(List<IDailyData> list)
        {
            foreach (IDailyData q in list)
                this.Add(q);
            _AddExtraDays(8);            
        }

        public void AddRange(DataCollection list)
        {
            foreach (IDailyData q in list)
                this.Add(q);
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

        #region IDisposable Members
        // Flag: Has Dispose already been called? 
        private decimal _DAYS_OF_SUPPLIES;
        private string _AVERAGE_TURNS_L4WK_Tooltip;
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
                this.Select(d => d).Where(d=>d.HOLIDAYS != null).Update(d => d.HOLIDAYS.Dispose());
                this.Select(d => d).Where(d => d.HOLIDAYS != null).Update(d => d.HOLIDAYS = null);
                if (PERFORMANCE_EXCEPTIONS != null)
                {
                    this.PERFORMANCE_EXCEPTIONS.Dispose();
                    this.PERFORMANCE_EXCEPTIONS = null;
                }
                if (TwoWeeksPriorData != null)
                {
                    if (TwoWeeksPriorData.HOLIDAYS != null)
                    {
                        this.TwoWeeksPriorData.HOLIDAYS.Dispose();
                        this.TwoWeeksPriorData = null;
                    }
                    TwoWeeksPriorData = null;
                }
                if (LastWeekData != null)
                {
                    if (LastWeekData.HOLIDAYS != null)
                    {
                        this.LastWeekData.HOLIDAYS.Dispose();
                        this.LastWeekData.HOLIDAYS = null;
                    }
                    this.LastWeekData = null;
                }
                if (FocusedData != null)
                {
                    if (FocusedData.HOLIDAYS != null)
                    {
                        this.FocusedData.HOLIDAYS.Dispose();
                        this.FocusedData.HOLIDAYS = null;
                    }
                    this.FocusedData = null;
                }
                if (FocusedDataCollection != null)
                {
                    this.FocusedDataCollection.Where(d => d.HOLIDAYS != null).Update(d => d.HOLIDAYS.Dispose());
                    this.FocusedDataCollection.Where(d => d.HOLIDAYS != null).Update(d => d.HOLIDAYS = null);
                    this.FocusedDataCollection.Update(d => d = null);
                }
                if (this.FSIS != null)
                    this.FSIS = null;
                if (this.CELL_COMMENTS != null)
                    this.CELL_COMMENTS = null;
                if (this.QDS != null)
                    this.QDS = null;
                if (this.QCS != null)
                    this.QCS = null;
                this.SKUID = null;
                this.CUSTOMER = null;
                this.PERFORMANCES = null;
                this.FORECASTS = null;
            }

            // Free any unmanaged objects here. 
            //
            //get rid of each dailydata holiday collection 
            
            
            disposed = true;
        }
        #endregion

        #region PROPERTIES
        
        private int _VIEW;
        private double _AVERAGE_TURNS_L4WK;
        private IDailyData _TwoWeeksPriorData;
        private IDailyData _LastWeekData;
        private bool _Updated_PERFORMANCES;
        private bool _IsUpdating_PERFORMANCES;
        private PerformanceExceptionsCollection _PERFORMANCE_EXCEPTIONS;
        private double _AVERAGE_TURNS;
        private int _MIN_YEAR;
        private int _MAX_YEAR;
        private DateTime _LASTDAYOFLASTWEEKWITHREALDATA;
        private DateTime _LASTDAYWITHREALDATA;
        private List<IFsi> _FSIS;
        private List<ICellComment> _CELL_COMMENTS;
        private IDailyData _FocusedData;
        private int _Number_Of_Years;
        private IEnumerable<IDailyData> _FocusedDataCollection;
        private List<IComment> _COMMENTS;
        private List<IQcQd> _QDS;
        private List<IQcQd> _QCS;
        private ISku _SKUID;
        private ICustomer _CUSTOMER;
        
        private PerformanceCollection _PERFORMANCES;

        private List<ForecastCollection> _FORECASTS;


        public ITab TAB { get; set; }
        /// <summary>
        /// NUMBER OF YEARS OF DATA LOADED
        /// </summary>
        public int Number_Of_Years
        {
            get
            {
                return _Number_Of_Years;
            }
            set
            {
                _Number_Of_Years = value;
                NotifyPropertyChanged();
            }
        }

        public int MAX_YEAR
        {
            get
            {
                return _MAX_YEAR;
            }
            set
            {
                _MAX_YEAR = value;
                NotifyPropertyChanged();
            }
        }

        public int MIN_YEAR
        {
            get
            {
                return _MIN_YEAR;
            }
            set
            {
                _MIN_YEAR = value;
                NotifyPropertyChanged();
            }
        }
        
        public IEnumerable<IDailyData> FocusedDataCollection
        {
            get
            {
                return _FocusedDataCollection;
            }
            set
            {
                _FocusedDataCollection = value;
                FocusedData = value != null ? value.ToList()[0] : null;
                LastWeekData = value != null && FocusedData != null ? this.Where(x =>  x.INDEX == FocusedData.INDEX + 1).FirstOrDefault() : null;
                TwoWeeksPriorData = value != null && FocusedData != null ? this.Where(x => x.INDEX == FocusedData.INDEX + 2).FirstOrDefault() : null;
                NotifyPropertyChanged();
            }
        }


        public IDailyData FocusedData
        {
            get
            {
                return _FocusedData;
            }
            private set
            {
                _FocusedData = value;
                NotifyPropertyChanged();
            }
        }

        public IDailyData LastWeekData
        {
            get
            {
                return _LastWeekData;
            }
            set
            {
                _LastWeekData = value;
                NotifyPropertyChanged();
            }
        }
        public IDailyData TwoWeeksPriorData
        {
            get
            {
                return _TwoWeeksPriorData;
            }
            set
            {
                _TwoWeeksPriorData = value;
                NotifyPropertyChanged();
            }
        }
                
        public System.DayOfWeek DAY_OF_WEEK { get; set; }
        public int TOTAL_QS { get; set; }
        public DateTime FIRSTDAYOFTHEWEEK { get; set; }
        public DateTime LASTDAYWITHREALDATA
        {
            get
            {
                return _LASTDAYWITHREALDATA;
            }
            set
            {
                _LASTDAYWITHREALDATA = value;                
                NotifyPropertyChanged();
            }
        }
        public DateTime LASTDAYOFLASTWEEKWITHREALDATA
        {
            get
            {
                return _LASTDAYOFLASTWEEKWITHREALDATA;
            }
            set
            {
                _LASTDAYOFLASTWEEKWITHREALDATA = value;
                NotifyPropertyChanged();
            }
        }



        public void SetLASTDAYOFLASTWEEKWITHREALDATA()
        {
            LASTDAYOFLASTWEEKWITHREALDATA = this.LASTDAYWITHREALDATA.lastDayOfLastWeek(_Date.DayOfWeek(this.CUSTOMER.ACCOUNTNUM));
        }
        /// <summary>
        /// GENERAL NUMBER OF DAYS IN A WEEK FOR THIS CUSTOMER
        /// HOW MANY DAYS IN A WEEK THIS CUSTOMER REPORTS
        /// </summary>
        public int NUMBEROFDAYSINWEEK { get; set; }
        /// <summary>
        /// NUMBER OF DAYS IN THE LAST REPORTING WEEK
        /// </summary>
        public int NUMBEROFDAYINLASTWEEK { get; set; }
        /// <summary>
        /// DAYS OF WEEK
        /// </summary>
        public IEnumerable<DayOfWeek> DAYSINWEEK { get; set; }
        

        public ICustomer CUSTOMER
        {
            get
            {
                return _CUSTOMER;
            }
            set
            {
                _CUSTOMER = value;
                //if (this.SKUID != null)
                //    this.Daily_QA_and_Weekly_QS = String.Format("{0} {1} Daily QA and Weekly QS", this.CUSTOMER.NAME, this.SKUID.SKUID);
                NotifyPropertyChanged();

            }
        }
        public ISku SKUID
        {
            get
            {
                return _SKUID;
            }
            set
            {
                _SKUID = value;
                //if (this.CUSTOMER != null)
                //    this.Daily_QA_and_Weekly_QS = String.Format("{0} {1} Daily QA and Weekly QS", this.CUSTOMER.NAME, this.SKUID.SKUID);
                NotifyPropertyChanged();
            }
        }

        public string SKU_COMMENT
        {
            get
            {
                if (SKUID == null || SKUID.SETTING == null) return string.Empty;
                return this.SKUID.SETTING.COMMENT;
            }            
        }


        public int VIEW
        {
            get
            {
                return _VIEW;
            }
            set
            {
                _VIEW = value;
                NotifyPropertyChanged();
            }
        }
        public List<ICellComment> CELL_COMMENTS
        {
            get
            {
                return _CELL_COMMENTS;
            }
            set
            {
                _CELL_COMMENTS = value;
                NotifyPropertyChanged();
            }
        }

        public List<IFsi> FSIS
        {
            get
            {
                return _FSIS;
            }
            set
            {
                _FSIS = value;
                NotifyPropertyChanged();
            }
        }

        public List<ForecastCollection> FORECASTS
        {
            get
            {
                return _FORECASTS;
            }
            set
            {
                _FORECASTS = value;
                NotifyPropertyChanged();
            }
        }
        public List<IQcQd> QCS
        {
            get
            {
                return _QCS;
            }
            set
            {
                _QCS = value;
                NotifyPropertyChanged();
            }
        }

        public List<IQcQd> QDS
        {
            get
            {
                return _QDS;
            }
            set
            {
                _QDS = value;
                NotifyPropertyChanged();
            }
        }

        public List<IComment> COMMENTS
        {
            get
            {
                return _COMMENTS;
            }
            set
            {
                _COMMENTS = value;
                NotifyPropertyChanged();
            }
        }

        public PerformanceCollection PERFORMANCES
        {
            get
            {
                return _PERFORMANCES;
            }
            set
            {
                _PERFORMANCES = value;
                NotifyPropertyChanged();
            }
        }

        public PerformanceExceptionsCollection PERFORMANCE_EXCEPTIONS
        {
            get
            {
                return _PERFORMANCE_EXCEPTIONS;
            }
            set
            {
                _PERFORMANCE_EXCEPTIONS = value;
                NotifyPropertyChanged();
            }
        }

        public bool isUpdating_PERFORMANCES
        {
            get
            {
                return _IsUpdating_PERFORMANCES;
            }
            set
            {
                _IsUpdating_PERFORMANCES = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsUpdated_PERFORMANCES
        {
            get
            {
                return _Updated_PERFORMANCES;
            }
            set
            {
                _Updated_PERFORMANCES = value;
                NotifyPropertyChanged();
            }
        }

        public double AVERAGE_TURNS
        {
            get
            {
                return _AVERAGE_TURNS;
            }
            set
            {
                _AVERAGE_TURNS = value;                
                NotifyPropertyChanged();
                if (MainWindow.myTopMenuViewModel != null)
                    MainWindow.myTopMenuViewModel.NotifyPropertyChanged("AVERAGE_TURN");
            }
        }

        public double AVERAGE_TURNS_L4WK
        {
            get
            {
                return _AVERAGE_TURNS_L4WK;
            }
            set
            {
                _AVERAGE_TURNS_L4WK = value;                
                NotifyPropertyChanged();
                if (MainWindow.myTopMenuViewModel != null)
                    MainWindow.myTopMenuViewModel.NotifyPropertyChanged("AVERAGE_TURN_L4WK");                
            }
        }

        public decimal DAYS_OF_SUPPLIES
        {
            get
            {
                return _DAYS_OF_SUPPLIES;
            }
            set
            {
                _DAYS_OF_SUPPLIES = value;
                NotifyPropertyChanged();
                if (MainWindow.myTopMenuViewModel != null)
                    MainWindow.myTopMenuViewModel.NotifyPropertyChanged("DAYS_OF_SUPPLIES");
            }
        }

        public bool IsDailyDataExpanded
        {
            get
            {
                if (TAB != null)
                    return TAB.Setting.IsDailyDataExpanded;
                return true;
            }
            set
            {
                TAB.Setting.IsDailyDataExpanded = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region PUBLIC

        public static DataCollection GetWeekly(ITab tab)
        {
            try
            {
                var dailyCollection = tab.myDailyData;
                DataCollection list = new DataCollection
                {
                    CUSTOMER = dailyCollection.CUSTOMER,
                    SKUID = dailyCollection.SKUID,
                    TAB = tab
                };
                //var weekids = dataCollection.Select(x => x.WEEK_ID);
                list.DAY_OF_WEEK = dailyCollection.DAY_OF_WEEK;
                list.MIN_YEAR = dailyCollection.MIN_YEAR;
                list.MAX_YEAR = dailyCollection.MAX_YEAR;
                //list.PERFORMANCES = dailyCollection.PERFORMANCES;
                list.FORECASTS = dailyCollection.FORECASTS;
                list.CELL_COMMENTS = dailyCollection.CELL_COMMENTS;
                list.LASTDAYWITHREALDATA = dailyCollection.LASTDAYWITHREALDATA;

                list.QCS = dailyCollection.QCS; //QcQd.Create(weekids, dataCollection.QCS, "QC");
                list.QDS = dailyCollection.QDS; //QcQd.Create(weekids, dataCollection.QDS, "QD");
                list.COMMENTS = dailyCollection.COMMENTS;
                list.FSIS = dailyCollection.FSIS;

                IEnumerable<IGrouping<string, IDailyData>> groups = 
                    dailyCollection.GroupBy(x => x.WEEK_ID);
                uint index = 0;
                foreach (IGrouping<string, IDailyData> group in groups)
                {
                    if (group.Key.Contains("4420152010RDA128"))
                    {
                        var s = group.Key;
                    }
                    var gPerfs = group.SelectMany(x => x.PERFORMANCES).Distinct();
                    var shipTo = group.Select(x => x.SHIP_TO_TP).FirstOrDefault();
                    var itemUPC = group.Select(x => x.ITEM_UPC).FirstOrDefault(); ;
                    var reportAsOf = group.Select(x => x.REPORT_AS_OF_DATE).FirstOrDefault().StartOfWeek(list.DAY_OF_WEEK);
                    var weekNum = reportAsOf.WeekNumber(_Date.DayOfWeek(shipTo));
                    var year = reportAsOf.Year;

                    DailyData data = new DailyData(true, weekNum, year, shipTo, itemUPC);
                    ((IDailyData)data).IS_FUTURE = group.All(g => g.IS_FUTURE);
                    //((IDailyData)data).ITEM_UPC = group.Select(x => x.ITEM_UPC).FirstOrDefault();
                    //((IDailyData)data).SHIP_TO_TP = group.Select(x => x.SHIP_TO_TP).FirstOrDefault();
                    ((IDailyData)data).DAILY_DATA = group.ToList();
                    ((IDailyData)data).REPORT_AS_OF_DATE = group.Select(x => x.REPORT_AS_OF_DATE).FirstOrDefault().StartOfWeek(list.DAY_OF_WEEK);
                    ((IDailyData)data).ALL_REPORT_AS_OF_DATE = group.Select(x => x.REPORT_AS_OF_DATE);
                    ((IDailyData)data).FIRST_DAY_OF_WEEK = ((IDailyData)data).REPORT_AS_OF_DATE;
                    //((IDailyData)data).DOCUMENTOBJECTID = Guid.NewGuid();
                    //((IDailyData)data).WEEK_NUMBER = ((IDailyData)data).REPORT_AS_OF_DATE.WeekNumber(_Date.DayOfWeek(((IDailyData)data).SHIP_TO_TP));
                    //((IDailyData)data).YEAR = ((IDailyData)data).REPORT_AS_OF_DATE.Year;

                    ((IDailyData)data).QP = group.Sum(x => x.QP);
                    ((IDailyData)data).QA = group.First(x => x.REPORT_AS_OF_DATE == group.Min(d => d.REPORT_AS_OF_DATE)).QA; // .Select(x => x.QA).FirstOrDefault();
                    ((IDailyData)data).QO = group.Sum(x => x.QO);
                    ((IDailyData)data).QONumberOfNoneZeros = group.Count(x => x.QO > 0);
                    ((IDailyData)data).QS = group.Sum(x => x.QS);
                    ((IDailyData)data).QW = group.Sum(x => x.QW);
                    ((IDailyData)data).UOM = group.Select(x => x.UOM).FirstOrDefault();
                    ((IDailyData)data).LYQS = group.Sum(x => x.LYQS);

                    var currentWeekComment = list.CELL_COMMENTS.Where(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID);
                    ((IDailyData)data).QA_COMMENT = currentWeekComment.First(x => x.FIELD == CellComment.Field.QA); 
                    ((IDailyData)data).QO_COMMENT = currentWeekComment.First(x => x.FIELD == CellComment.Field.QO); 
                    ((IDailyData)data).QS_COMMENT = currentWeekComment.First(x => x.FIELD == CellComment.Field.QS); 
                    ((IDailyData)data).QW_COMMENT = currentWeekComment.First(x => x.FIELD == CellComment.Field.QW); 
                    ((IDailyData)data).LY_COMMENT = currentWeekComment.First(x => x.FIELD == CellComment.Field.LY);
                    if (currentWeekComment.Any(x => x.FIELD == CellComment.Field.BASE_LINE))
                        ((IDailyData)data).BASE_LINE.COMMENT.VALUE = currentWeekComment.First(x => x.FIELD == CellComment.Field.BASE_LINE).VALUE;
                    ((IDailyData)data).ORDER_DELIVERY_COMMENT = currentWeekComment.First(x => x.FIELD == CellComment.Field.ORDER); //.FirstOrDefault();

                    ((IDailyData)data).PERFORMANCES = PerformanceCollection.New(gPerfs, ((IDailyData)data).WEEK_ID, ((IDailyData)data).REPORT_AS_OF_DATE, ((IDailyData)data).ALL_REPORT_AS_OF_DATE);
                    //((IDailyData)data).PERFORMANCES = PerformanceCollection.New(list.PERFORMANCES.Where(x => x.START_DATE <= ((IDailyData)data).REPORT_AS_OF_DATE && x.END_DATE >= ((IDailyData)data).REPORT_AS_OF_DATE), ((IDailyData)data).WEEK_ID);
                    ((IDailyData)data).FORECASTS = list.FORECASTS.First(x => x.WEEKID == ((IDailyData)data).WEEK_ID); //.FirstOrDefault();
                    if (((IDailyData)data).FORECASTS.LASTENTERED != null)
                        ((IDailyData)data).BASE_LINE.VALUE =
                            ((IDailyData)data).FORECASTS.LASTENTERED.BASELINE;
                    ((IDailyData)data).QC = list.QCS.First(x => x.WEEKID == ((IDailyData)data).WEEK_ID); //.FirstOrDefault();
                    ((IDailyData)data).QD = list.QDS.First(x => x.WEEKID == ((IDailyData)data).WEEK_ID); //.FirstOrDefault();

                    ((IDailyData)data).COMMENT = list.COMMENTS.First(x => x.WEEKID == ((IDailyData)data).WEEK_ID); //.FirstOrDefault();
                    ((IDailyData)data).FSI = list.FSIS.First(x => x.WEEKID == ((IDailyData)data).WEEK_ID); //.FirstOrDefault();

                    ((IDailyData)data).HOLIDAYS = MainWindow.myHolidays.GetListForDays(((IDailyData)data).ALL_DAYS_IN_WEEK);
                    ((IDailyData)data).INDEX = index++;
                    list.Add(((IDailyData)data));
                }
                list.PERFORMANCES = new PerformanceCollection(list.SelectMany(x => x.PERFORMANCES).Distinct());
                
                list.SetLASTDAYOFLASTWEEKWITHREALDATA();
                list.GetAverageTurn();
                list.GetDaysOfSuppliesAsync();
                return list;
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                throw new ApplicationException("Unable to get the weekly data");
            }
        }

        public void GetDaily(string customerId, ITab tab, int timeLastQueried, int view)
        {
            try
            {
                int custId = int.Parse(customerId);
                switch (view)
                {
                    case 0:
                        List<VMI_Get_Customers_Quantity_Amounts_Result> query;
                        using (var context = new dbidbEntities())
                        {
                            query = context.VMI_Get_Customers_Quantity_Amounts(custId.ToString(), tab.SKU.SKUID, timeLastQueried).ToList();
                        }
                        //IF NO 852 DATA ARE RETURNED
                        if (query == null || query.Count() == 0)
                        {
                            tab.SetError("No 852 data was found");
                            if (query == null) query = new List<VMI_Get_Customers_Quantity_Amounts_Result>();
                            query.AddRange(_BuidEmpty852RecordByYear(custId));
                        }
                        else
                        {
                            //IF 852 DATA DONT INCLUDE LAST WEEK
                            var days = Tools.GetWeekDates(DateTime.Now.AddDays(-7));
                            if (!query.Any(i => days.Contains(i.ReportAsOfDate)))
                            {
                                tab.SetError("An empty record was added to 852 records for the last week");
                                query.AddRange(_BuidEmpty852RecordByYear(custId));
                            }
                            _GetDailyByYear(custId, tab, query.OrderByDescending(i => i.ReportAsOfDate).ToList());
                            break;
                        }

                        _GetDailyByYear(custId, tab, query);
                        break;
                    case 1:
                        List<VMI_Get_Customers_Quantity_Amounts_Up_Last_Date_Result> query2;
                        var lastDayToQuery = DateTime.Now.AddMonths(-timeLastQueried).StartOfWeek(_Date.DayOfWeek(customerId));
                        using (var context = new dbidbEntities())
                        {
                            query2 = context.VMI_Get_Customers_Quantity_Amounts_Up_Last_Date(custId.ToString(), tab.SKU.SKUID, lastDayToQuery).ToList();
                        }
                        //IF NO 852 DATA ARE RETURNED
                        if (query2 == null || query2.Count() == 0)
                        {
                            tab.SetError("No 852 data was found");
                            if (query2 == null) query2 = new List<VMI_Get_Customers_Quantity_Amounts_Up_Last_Date_Result>();
                            query2.AddRange(_BuidEmpty852RecordByPeriod(custId));
                        }
                        else
                        {
                            //IF 852 DATA DONT INCLUDE LAST WEEK
                            var days = Tools.GetWeekDates(DateTime.Now.AddDays(-7));
                            if (!query2.Any(i => days.Contains(i.ReportAsOfDate)))
                            {
                                tab.SetError("An empty record was added to 852 records for the last week");
                                query2.AddRange(_BuidEmpty852RecordByPeriod(custId));
                            }
                        }
                        _GetDailyByPeriod(custId, tab, query2);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                throw new ApplicationException(String.Format("Unable to get the daily data ({0})", ex.Message));
            }
        }

        public void UpdatePerformance(FORECASTER_PERFORMANCES_EXCEPTIONS item)
        {
            var ifAny = this.SelectMany(x => x.PERFORMANCES).Where(x => x.DOCUMENTOBJECTID == item.GUID);
            var originalWeeks = this.Where(x => x.PERFORMANCES.Any(p => p.DOCUMENTOBJECTID == item.GUID));
            var newWeeks = this.Where(x => Tools.GetWeekDates(item.FROM, item.TO).Contains(x.FIRST_DAY_OF_WEEK));
            var addedWeeks = newWeeks.Where(x => !originalWeeks.Select(i => i.WEEK_ID).Contains(x.WEEK_ID));
            var removedWeeks = originalWeeks.Where(x => !newWeeks.Select(i => i.WEEK_ID).Contains(x.WEEK_ID));

            if (ifAny != null && ifAny.Count() > 0)
            {
                var myP = ifAny.First();
                myP.START_DATE = item.FROM;
                myP.PRICE = item.PRICE;
                myP.QUANTITY = item.QUANTITY;
                myP.END_DATE = item.TO;
                myP.PERFORMANCE_TYPE = item.TYPE;
                myP.D_STATE = (Controlers.Enums.DState)item.STATE;
                myP.M_STATE = (Controlers.Enums.MState)item.STATE;
                if (!item.IS_DEFAULT)
                {
                    foreach (var data in addedWeeks)
                        data.PERFORMANCES.Add(myP);
                    foreach (var data in removedWeeks)
                    {
                        var perfs = data.PERFORMANCES.Where(i => i.DOCUMENTOBJECTID == item.GUID);
                        if (perfs != null && perfs.Count() > 0)
                            data.PERFORMANCES.Remove(perfs.First());
                    }
                }

                //this.Update(x => x.UPDATE_PERFORMANCES());                
                this.Update(x => x.OnPropertyChanged(null));
                this.GetAverageTurn();
                var tab = MainWindow.myMainWindowViewModel.TABS.GetTab(this.SKUID.SKUID);
                if (tab.myPercentageViewModel != null)
                    tab.myPercentageViewModel.Update();
                if (tab.myGraphViewModel != null)
                {
                    tab.myGraphViewModel.SetQSStatisticalData();
                    tab.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS += 0;
                    tab.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS += 0;
                    tab.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS += 0;
                }
            }
        }

        public void AddPerformance(IPerformance item)
        {
            var ifAny = this.SelectMany(x => x.PERFORMANCES).Where(x => x.DOCUMENTOBJECTID == item.DOCUMENTOBJECTID);

            if (ifAny != null && ifAny.Count() > 0)
            {//there is a performance like this already. just include it.
                ifAny.Update(x => x.IS_INCLUDED = true);
                //this.Update(x => x.UPDATE_PERFORMANCES());
                this.Update(x => x.OnPropertyChanged(null));
            }
            else
            {
                var data = this.Where(x => x.ALL_DAYS_IN_WEEK.Any(d => d >= item.START_DATE && d <= item.END_DATE));
                data.Update(x => x.AddPerformance(item));
                //data.Update(x => x.UPDATE_PERFORMANCES());
                data.Update(x => x.OnPropertyChanged(null));
            }
            this.GetAverageTurn();
            var tab = MainWindow.myMainWindowViewModel.TABS.GetTab(this.SKUID.SKUID);
            if (tab.myPercentageViewModel != null)
                tab.myPercentageViewModel.Update();
            if (tab.myGraphViewModel != null)
            {
                tab.myGraphViewModel.SetQSStatisticalData();
                tab.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS += 0;
                tab.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS += 0;
                tab.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS += 0;
            }

        }

        public void RemovePerformance(Guid guid)
        {
            //GET LIST OF DAILYDATA THAT HAVE PERFORMANCES WITH THIS GUID ID
            var data = this.Where(x => x.PERFORMANCES.Any(i => i.DOCUMENTOBJECTID == guid));
            //GET LIST OF PERFORMANCES FOR THE ABOVE LIST OF DAILYDATA
            var ifAny = this.SelectMany(x => x.PERFORMANCES).Where(x => x.DOCUMENTOBJECTID == guid);
            //IF ANY PERFROMANCES EXIST
            if (ifAny != null && ifAny.Count() > 0)
            {
                var toRemove = new PerformanceCollection();
                //TAG IT TO NOT INCLUDED AND ADD IT TO A LIST FOR REMOVAL
                foreach (var p in ifAny)
                {
                    if (p.IS_DEFAULT)
                        p.IS_INCLUDED = false;
                    else
                        toRemove.Add(p);
                }
                //DELETE IT FROM THE PERFORMANCE COLLECTION
                foreach (var p in toRemove)
                {
                    data.Update(x => x.DeletePerformance(p));
                    var perf = this.PERFORMANCES.Where(i => i.DOCUMENTOBJECTID == guid).FirstOrDefault();
                    if (perf != null)
                        this.PERFORMANCES.Remove(perf);
                }
                //UPDATE THE PERFORMANCE COLLECTION. THIS WILL RESET THE FLAG THAT TELLS IF THERE ARE ANY PERFORMANCES FOR THIS ITEM
                //this.Update(x => x.UPDATE_PERFORMANCES());
                //UPDATE THE AVERAGE TURN. AT DEPENDS ON PERFORMANCES
                this.GetAverageTurn();
                //UPDATE THE ENTIRE COLLECTION
                this.Update(x => x.OnPropertyChanged(null));
                //UPDATE THE PERCENTAGE VALUES. THIS DEPENDS ON PERFORMANCES
                var tab = MainWindow.myMainWindowViewModel.TABS.GetTab(this.SKUID.SKUID);
                if (tab.myPercentageViewModel != null)
                    tab.myPercentageViewModel.Update();
                //UPDATE ALL GRAPHS
                if (tab.myGraphViewModel != null)
                {
                    tab.myGraphViewModel.SetQSStatisticalData();
                    tab.myGraphViewModel.DAILY_QA_WEEKLY_QS_NUMBER_OF_WEEKS += 0;
                    tab.myGraphViewModel.WEEKLY_POS_NUMBER_OF_WEEKS += 0;
                    tab.myGraphViewModel.WEEKLY_QS_ORDER_QUANTITY_NUMBER_OF_WEEKS += 0;
                }

            }
        }

        public void UpdatePerformances(PerformanceExceptionsCollection exceptions)
        {
            if (exceptions == null) return;
            this.isUpdating_PERFORMANCES = true;

            //GET ALL EXCEPTIONS BELONGING TO SALES PERFORMANCES
            var trueItems = exceptions.Where(x =>x.IS_DEFAULT == true && x.CUSTOMER_ID == int.Parse(this.CUSTOMER.ACCOUNTNUM) && x.SKU == this.SKUID.SKUID);
            foreach (var item in trueItems)
            {
                var Performances = this.SelectMany(x => x.PERFORMANCES.Where(p => p.DOCUMENTOBJECTID == item.GUID));
                if (Performances == null || Performances.Count() == 0) continue;
                Performances.Update(x => x.IS_INCLUDED = item.INCLUDE);
                Performances.Update(p => p.D_STATE = (DState)(item.STATE ?? 0));
                 Performances.Update(p => p.STATE_LAST_UPDATED = item.STATE_LAST_UPDATED ?? DateTime.Now);
                Performances.Update(p => p.STATE_USER = item.STATE_USER ?? string.Empty);
                //this.Update(x => x.UPDATE_PERFORMANCES ());
            }

            //GET ALL EXCEPTIONS THAT ARE GENERATED BY CUSTOMER RELATIONS
            var falseItems = exceptions.Where(x => x.IS_DEFAULT == false && x.CUSTOMER_ID == int.Parse(this.CUSTOMER.ACCOUNTNUM) && x.SKU == this.SKUID.SKUID);
            foreach (var item in falseItems)
            {
                var ex = new Performance
                {
                    AD_NAME = "Custom",
                    START_DATE = item.FROM,
                    END_DATE = item.TO,
                    CUSTOMER_ID = item.CUSTOMER_ID.AddLeadingZeros(),
                    TYPE = item.TYPE,
                    TYPE_ID = Performance.GetTypeId(item.TYPE),
                    DOCUMENTOBJECTID = item.GUID,
                    QUANTITY = item.QUANTITY,
                    PRICE = item.PRICE,
                    IS_INCLUDED = true,
                    IS_DEFAULT = false,
                    STATE_LAST_UPDATED = item.STATE_LAST_UPDATED ?? DateTime.Now,
                    STATE_USER = item.STATE_USER ?? string.Empty,
                    M_STATE = (MState)(item.STATE ?? 0)
                };
                var data = this.Where(x => x.ALL_DAYS_IN_WEEK.Any(d => d >= ex.START_DATE && d <= ex.END_DATE));
                data.Update(x => x.PERFORMANCES.Add(ex));
                //data.Update(x => x.UPDATE_PERFORMANCES());
            }
            this.IsUpdated_PERFORMANCES = false;

        }

        public static bool IsCollectionNull(DataCollection dataColl)
        {
            if (dataColl == null) return true;
            return false;
        }

        public double GetQSPercentage(System.DayOfWeek dayOfWeek, int numberOfPeriods)
        {
            var list1 = this.Where(x => x.REPORT_AS_OF_DATE <= this.LASTDAYOFLASTWEEKWITHREALDATA)
                .Where(x => !x.HAS_HOLIDAY && !x.HAS_PERFORMANCES)
                .SelectMany(x => x.DAILY_DATA)
                .OrderByDescending(x => x.REPORT_AS_OF_DATE).Take(numberOfPeriods);
            var list2 = list1.Where(x => string.Compare(dayOfWeek.ToString(), x.DAY_OF_WEEK, true) == 0);
            var sumOfDays = Convert.ToDouble(list2.Sum(x => x.QS));
            var sumOfAllDays = Convert.ToDouble(list1.Sum(x => x.QS));
            return sumOfDays / sumOfAllDays;
        }

        public void GetAverageTurn()
        {
            try
            {
                this.AVERAGE_TURNS = _GetAverageTurn(13);
                this.AVERAGE_TURNS_L4WK = _GetAverageTurn(4);
                return;
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to get average turn");
            }
            this.AVERAGE_TURNS = 0;
            this.AVERAGE_TURNS_L4WK = 0;
        }

        public async void GetDaysOfSuppliesAsync ()
        {
            await Task.Run(() =>
            {
                var coll =
                this.Where(x => x.REPORT_AS_OF_DATE >= this.LASTDAYOFLASTWEEKWITHREALDATA)
                    .OrderBy(x => x.REPORT_AS_OF_DATE)
                    .Take(4);
                var reportingDays = this.CUSTOMER.SETTING.REPORT_DAYS;
                var forcasts = coll.Select(c => new
                {
                    days = c.DAILY_DATA.Count(d=>d.REPORT_AS_OF_DATE >= DateTime.Now.Date),
                    forecast = c.FORECASTS,
                    date = c.REPORT_AS_OF_DATE
                });
                var dailyForecast = forcasts.Select(f =>
                    new
                    {
                        dailyF = (
                            f.forecast.LASTENTERED == null
                                ? 0
                                : Convert.ToDecimal(f.forecast.LASTVALUE) / f.days),
                        date = f.date,
                        days = f.days
                    });

                var lastQA = coll.First().DAILY_DATA.Where(d=>!d.IS_FUTURE).First().QA;
                decimal result = 0;
                decimal remainQA = lastQA;
                decimal dF = 0;
                int counter = -1;
                int counter2 = 0;
                foreach (var df in dailyForecast)
                {
                    counter2++;
                    for (int i = 1; i <= df.days; i++)
                    {
                        if (counter2 == 1 && counter == -1)
                        {
                            counter++;
                        }
                        else
                        {
                            if (remainQA <= 0)
                            {
                                result = counter + (df.dailyF > 0 ? (remainQA / df.dailyF) : 0);
                                this.DAYS_OF_SUPPLIES = Math.Round(result, 2);
                                return;
                            }
                            remainQA = remainQA - df.dailyF;
                            counter++;
                        }
                    }
                    
                }
            });
            
            
          
        }
        #endregion

        #region PRIVATE
        List<datastores.VMI_Get_Customers_Quantity_Amounts_Up_Last_Date_Result> _BuidEmpty852RecordByPeriod(int custId)
        {
            var query2 = new List<datastores.VMI_Get_Customers_Quantity_Amounts_Up_Last_Date_Result>();
            List<DateTime> missinDaysDate = null;
            List<datastores.FORECASTER_CUSTOMER_SETTINGS> settings;
            using (var context = new datastores.ForecasterEntities(null))
            {
                settings = context.FORECASTER_CUSTOMER_SETTINGS.Where(c => c.CUSTOMER_ID == custId).ToList();
            }
            if (settings != null && settings.Count() > 0)
            {
                var first = settings.First();
                var days = Tools.GetDaysOfWeek(first.REPORTING_START, settings.First().REPORTING_END);
                this.DAYSINWEEK = days;
                this.MAX_YEAR = DateTime.Now.Year;
                this.MIN_YEAR = DateTime.Now.Year;
                this.DAY_OF_WEEK = _Date.DayOfWeek(custId.ToString());
                this.NUMBEROFDAYSINWEEK = days.Count();
                this.LASTDAYWITHREALDATA = DateTime.Now;
                //ADD ALL MISSING DAYS FOR THIS WEEK
                var missingDays = days.Where(d => (int)d != (int)DateTime.Now.DayOfWeek);
                missinDaysDate = Tools.GetDates(days, DateTime.Now.AddDays(-7));
                if (missinDaysDate != null && missinDaysDate.Count() > 0)
                {
                    foreach (var date in missinDaysDate)
                    {
                        string[] activities = new string[] { "QA", "QS", "QW", "QO" };
                        foreach (var s in activities)
                        {
                            var item = new datastores.VMI_Get_Customers_Quantity_Amounts_Up_Last_Date_Result
                            {
                                ActivityCode = s,
                                Quantity = 0,
                                ShipToTP = custId.ToString(),
                                ReportAsOfDate = date,
                            };
                            query2.Add(item);
                        }
                    }
                }
            }
            return query2;
        }
        List<datastores.VMI_Get_Customers_Quantity_Amounts_Result> _BuidEmpty852RecordByYear(int custId)
        {
            var query2 = new List<datastores.VMI_Get_Customers_Quantity_Amounts_Result>();
            List<DateTime> missinDaysDate = null;
            List<datastores.FORECASTER_CUSTOMER_SETTINGS> settings;
            using (var context = new datastores.ForecasterEntities(null))
            {
                settings = context.FORECASTER_CUSTOMER_SETTINGS.Where(c => c.CUSTOMER_ID == custId).ToList();
            }
            if (settings != null && settings.Count() > 0)
            {
                var first = settings.First();
                var days = Tools.GetDaysOfWeek(first.REPORTING_START, settings.First().REPORTING_END);
                this.DAYSINWEEK = days;
                this.MAX_YEAR = DateTime.Now.Year;
                this.MIN_YEAR = DateTime.Now.Year;
                this.DAY_OF_WEEK = _Date.DayOfWeek(custId.ToString());
                this.NUMBEROFDAYSINWEEK = days.Count();
                this.LASTDAYWITHREALDATA = DateTime.Now;
                //ADD ALL MISSING DAYS FOR THIS WEEK
                var missingDays = days.Where(d => (int)d != (int)DateTime.Now.DayOfWeek);
                missinDaysDate = Tools.GetDates(days, DateTime.Now.AddDays(-7));
                if (missinDaysDate != null && missinDaysDate.Count() > 0)
                {
                    foreach (var date in missinDaysDate)
                    {
                        string[] activities = new string[] { "QA", "QS", "QW", "QO" };
                        foreach (var s in activities)
                        {
                            var item = new datastores.VMI_Get_Customers_Quantity_Amounts_Result
                            {
                                ActivityCode = s,
                                Quantity = 0,
                                ShipToTP = custId.ToString(),
                                ReportAsOfDate = date,
                            };
                            query2.Add(item);
                        }
                    }
                }
            }
            return query2;
        }

        void _GetDailyByYear(int custId, ITab tab, List<VMI_Get_Customers_Quantity_Amounts_Result> query)
        {

            //var query = dt.GetData("1453", "rc1620", yearLastQueried);

            var skuShipToList = query.Select(x => x.ItemUPC + x.ShipToTP).Distinct();

            this.MAX_YEAR = query.Max(x => x.ReportAsOfDate.Year);
            this.MIN_YEAR = query.Min(x => x.ReportAsOfDate.Year);

            this.DAY_OF_WEEK = _Date.DayOfWeek(custId.ToString());
            this.FORECASTS = ForecastCollection.Get(skuShipToList, int.Parse(tab.CUSTOMER_NUMBER));
            var QCQDs = QcQd.GetCollection(skuShipToList, int.Parse(tab.CUSTOMER_NUMBER));
            this.QCS = QCQDs.Where(x => x.FIELD == "QC").ToList();
            this.QDS = QCQDs.Where(x => x.FIELD == "QD").ToList();
            this.COMMENTS = Comment.GetCollection(skuShipToList, int.Parse(tab.CUSTOMER_NUMBER));
            this.CELL_COMMENTS = CellComment.GetCollection(skuShipToList, int.Parse(tab.CUSTOMER_NUMBER));
            this.PERFORMANCES = PerformanceCollection.Get(custId, tab.SKU.SKUID);
            this.FSIS = Fsi.Get(custId.AddLeadingZeros(), tab.SKU.SKUID);

            var qa = query.Where(x => x.ActivityCode == "QA");
            var qs = query.Where(x => x.ActivityCode == "QS");
            var qw = query.Where(x => x.ActivityCode == "QW");
            var qo = query.Where(x => x.ActivityCode == "QO");
            var qp = query.Where(x => x.ActivityCode == "QP");
            List<IDailyData> list = new List<IDailyData>();
            var objId = query.Select(x => x.ReportAsOfDate).Distinct();

            foreach (DateTime g in objId)
            {
                var temp = query.First(x => x.ReportAsOfDate == g);
                var shipTo = temp.ShipToTP;
                var itemUPC = temp.ItemUPC;
                
                var reportAsOf = temp.ReportAsOfDate;
                var firstDay = reportAsOf.StartOfWeek(this.DAY_OF_WEEK);
                var weekNum = firstDay.WeekNumber(_Date.DayOfWeek(shipTo));
                var year = firstDay.Year;

                var data = new DailyData(true, weekNum, year, shipTo, itemUPC);

                //((IDailyData)data).ITEM_UPC = temp.ItemUPC;
                //((IDailyData)data).SHIP_TO_TP = temp.ShipToTP;
                ((IDailyData)data).REPORT_AS_OF_DATE = reportAsOf;
                ((IDailyData)data).ALL_REPORT_AS_OF_DATE = new List<DateTime>() { ((IDailyData)data).REPORT_AS_OF_DATE };
                ((IDailyData)data).FIRST_DAY_OF_WEEK = firstDay;
                //((IDailyData)data).WEEK_NUMBER = ((IDailyData)data).FIRST_DAY_OF_WEEK.WeekNumber(_Date.DayOfWeek(((IDailyData)data).SHIP_TO_TP));
                //((IDailyData)data).YEAR = ((IDailyData)data).FIRST_DAY_OF_WEEK.Year;

                if (((IDailyData)data).FIRST_DAY_OF_WEEK.Year < this.MIN_YEAR) continue;

                var currentWeekComment = this.CELL_COMMENTS.Where(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID);

                ((IDailyData)data).QP = qp.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                ((IDailyData)data).QA = qa.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                ((IDailyData)data).QA_COMMENT = currentWeekComment.FirstOrDefault(x => x.FIELD == CellComment.Field.QA) ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QA);
                ((IDailyData)data).QO = qo.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                ((IDailyData)data).QO_COMMENT = currentWeekComment.FirstOrDefault(x => x.FIELD == CellComment.Field.QO) ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QO);
                ((IDailyData)data).QS = qs.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                ((IDailyData)data).QS_COMMENT = currentWeekComment.FirstOrDefault(x => x.FIELD == CellComment.Field.QS) ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QS);
                ((IDailyData)data).QW = qw.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                ((IDailyData)data).QW_COMMENT = currentWeekComment.FirstOrDefault(x => x.FIELD == CellComment.Field.QW) ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QW);
                ((IDailyData)data).ORDER_DELIVERY_COMMENT = currentWeekComment.FirstOrDefault(x => x.FIELD == CellComment.Field.ORDER) ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.ORDER);

                var forecastComments = currentWeekComment.Where(x => x.FIELD == CellComment.Field.FORECAST);

                ((IDailyData)data).UOM = temp.UOM;
                ((IDailyData)data).LY_COMMENT = this.CELL_COMMENTS.FirstOrDefault(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID && x.FIELD == CellComment.Field.LY) ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.LY);

                ((IDailyData)data).LYQW = qw.Where(x => x.ReportAsOfDate == ((IDailyData)data).LAST_YEAR_DATE).Select(x => x.Quantity).FirstOrDefault();


                ((IDailyData)data).PERFORMANCES = PerformanceCollection.New(this.PERFORMANCES.Where(x => x.START_DATE <= ((IDailyData)data).REPORT_AS_OF_DATE && x.END_DATE >= ((IDailyData)data).REPORT_AS_OF_DATE),
                    ((IDailyData)data).WEEK_ID, ((IDailyData)data).REPORT_AS_OF_DATE);


                ((IDailyData)data).COMMENT = this.COMMENTS.FirstOrDefault(x => x.WEEKID == ((IDailyData)data).WEEK_ID) ?? Comment.AddEmpty(this.COMMENTS, ((IDailyData)data).WEEK_ID);
                ((IDailyData)data).FSI = this.FSIS.FirstOrDefault(x => x.WEEKID == ((IDailyData)data).WEEK_ID) ?? Fsi.AddEmpty(this.FSIS, ((IDailyData)data).WEEK_ID);

                if (!this.FORECASTS.Any(x => x.WEEKID == (((IDailyData)data).WEEK_ID))) this.FORECASTS.Add(ForecastCollection.GetEmpty(new Forecast(((IDailyData)data).WEEK_ID)));
                ((IDailyData)data).FORECASTS = this.FORECASTS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault(); ;
                if (!forecastComments.Any(x => x.WEEK_ID == (((IDailyData)data).WEEK_ID))) ((IDailyData)data).FORECASTS.COMMENT = new CellComment(((IDailyData)data).WEEK_ID, CellComment.Field.FORECAST, string.Empty);
                else ((IDailyData)data).FORECASTS.COMMENT = forecastComments.FirstOrDefault(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID);


                var QC = this.QCS.FirstOrDefault(x => x.WEEKID == ((IDailyData)data).WEEK_ID);
                var QD = this.QDS.FirstOrDefault(x => x.WEEKID == ((IDailyData)data).WEEK_ID);

                ((IDailyData)data).QC = QC ?? QcQd.AddEmpty(this.QCS, ((IDailyData)data).WEEK_ID);
                ((IDailyData)data).QD = QD ?? QcQd.AddEmpty(this.QDS, ((IDailyData)data).WEEK_ID);

                ((IDailyData)data).HOLIDAYS = MainWindow.myHolidays.GetItemForDay(((IDailyData)data).REPORT_AS_OF_DATE);
                list.Add((IDailyData)data);
            }
            if (list.Count > 0)
            {
                this.AddDays(list);
                this.AddLYQS(tab.CUSTOMER_NUMBER, tab.SKU.SKUID, this.Min(x => x.REPORT_AS_OF_DATE), this.Max(x => x.REPORT_AS_OF_DATE));
            }
        }
        void _GetDailyByPeriod(
            int custId,
            ITab tab, 
            List<VMI_Get_Customers_Quantity_Amounts_Up_Last_Date_Result> query)
        {
            List<IDailyData> list = new List<IDailyData>();
            if (query != null && query.Count() > 0)
            {

                var skuShipToList = query.Select(x => x.ItemUPC + x.ShipToTP).Distinct();

                this.MAX_YEAR = query.Max(x => x.ReportAsOfDate.Year);
                this.MIN_YEAR = query.Min(x => x.ReportAsOfDate.Year);

                this.DAY_OF_WEEK = _Date.DayOfWeek(custId.ToString());
                this.FORECASTS = ForecastCollection.Get(skuShipToList, int.Parse(tab.CUSTOMER_NUMBER));
                var QCQDs = QcQd.GetCollection(skuShipToList, int.Parse(tab.CUSTOMER_NUMBER));
                this.QCS = QCQDs.Where(x => x.FIELD == "QC").ToList();
                this.QDS = QCQDs.Where(x => x.FIELD == "QD").ToList();
                this.COMMENTS = Comment.GetCollection(skuShipToList, int.Parse(tab.CUSTOMER_NUMBER));
                this.CELL_COMMENTS = CellComment.GetCollection(skuShipToList, int.Parse(tab.CUSTOMER_NUMBER));
                this.PERFORMANCES = PerformanceCollection.Get(custId, tab.SKU.SKUID);
                this.FSIS = Fsi.Get(custId.AddLeadingZeros(), tab.SKU.SKUID);

                var qa = query.Where(x => x.ActivityCode == "QA");
                var qs = query.Where(x => x.ActivityCode == "QS");
                var qw = query.Where(x => x.ActivityCode == "QW");
                var qo = query.Where(x => x.ActivityCode == "QO");
                var qp = query.Where(x => x.ActivityCode == "QP");

                var objId = query.Select(x => x.ReportAsOfDate).Distinct();

                foreach (DateTime g in objId)
                {
                    var temp = query.First(x => x.ReportAsOfDate == g);
                    var shipTo = temp.ShipToTP;
                    var itemUPC = temp.ItemUPC;
                    var reportAsOf = temp.ReportAsOfDate;
                    var firstDay = reportAsOf.StartOfWeek(this.DAY_OF_WEEK);
                    var weekNum = firstDay.WeekNumber(_Date.DayOfWeek(shipTo));
                    var year = firstDay.Year;

                    var data = new DailyData(true, weekNum, year, shipTo, itemUPC);
                    //data.RefreshDataContext += q_RefreshDataContext;

                    //((IDailyData)data).ITEM_UPC = temp.ItemUPC;
                    //((IDailyData)data).SHIP_TO_TP = temp.ShipToTP;
                    ((IDailyData)data).REPORT_AS_OF_DATE = reportAsOf;
                    ((IDailyData)data).ALL_REPORT_AS_OF_DATE = new List<DateTime>() { ((IDailyData)data).REPORT_AS_OF_DATE };
                    ((IDailyData)data).FIRST_DAY_OF_WEEK = firstDay;
                    //((IDailyData)data).WEEK_NUMBER = ((IDailyData)data).FIRST_DAY_OF_WEEK.WeekNumber(_Date.DayOfWeek(((IDailyData)data).SHIP_TO_TP));
                    //((IDailyData)data).YEAR = ((IDailyData)data).FIRST_DAY_OF_WEEK.Year;

                    //IF THE WEEK STARTING DAY FALLS INTO THE YEAR PRIOR TO LOWEST YEAR, DO NOT ADD
                    if (((IDailyData)data).FIRST_DAY_OF_WEEK.Year < this.MIN_YEAR) continue;

                    var currentWeekComment = this.CELL_COMMENTS.Where(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID);

                    ((IDailyData)data).QP = qp.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                    ((IDailyData)data).QA = qa.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                    ((IDailyData)data).QA_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.QA).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QA);
                    ((IDailyData)data).QO = qo.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                    ((IDailyData)data).QO_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.QO).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QO);
                    ((IDailyData)data).QS = qs.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                    ((IDailyData)data).QS_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.QS).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QS);
                    ((IDailyData)data).QW = qw.Where(x => x.ReportAsOfDate == g).Select(x => x.Quantity).Sum();
                    ((IDailyData)data).QW_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.QW).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QW);
                    ((IDailyData)data).ORDER_DELIVERY_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.ORDER).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.ORDER);

                    var forecastComments = currentWeekComment.Where(x => x.FIELD == CellComment.Field.FORECAST);

                    ((IDailyData)data).UOM = temp.UOM;
                    ((IDailyData)data).LYQS = qs.Where(x => x.ReportAsOfDate == ((IDailyData)data).LAST_YEAR_DATE).Select(x => x.Quantity).FirstOrDefault();
                    ((IDailyData)data).LY_COMMENT = this.CELL_COMMENTS.Where(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID && x.FIELD == CellComment.Field.LY).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.LY);

                    ((IDailyData)data).LYQW = qw.Where(x => x.ReportAsOfDate == ((IDailyData)data).LAST_YEAR_DATE).Select(x => x.Quantity).FirstOrDefault();


                    ((IDailyData)data).PERFORMANCES = PerformanceCollection.New(this.PERFORMANCES.Where(x => x.START_DATE <= ((IDailyData)data).REPORT_AS_OF_DATE && x.END_DATE >= ((IDailyData)data).REPORT_AS_OF_DATE),
                        ((IDailyData)data).WEEK_ID, ((IDailyData)data).REPORT_AS_OF_DATE);


                    ((IDailyData)data).COMMENT = this.COMMENTS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault() ?? Comment.AddEmpty(this.COMMENTS, ((IDailyData)data).WEEK_ID);
                    ((IDailyData)data).FSI = this.FSIS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault() ?? Fsi.AddEmpty(this.FSIS, ((IDailyData)data).WEEK_ID);

                    if (!this.FORECASTS.Any(x => x.WEEKID == (((IDailyData)data).WEEK_ID))) this.FORECASTS.Add(ForecastCollection.GetEmpty(new Forecast(((IDailyData)data).WEEK_ID)));
                    ((IDailyData)data).FORECASTS = this.FORECASTS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault();
                    if (!forecastComments.Any(x => x.WEEK_ID == (((IDailyData)data).WEEK_ID))) ((IDailyData)data).FORECASTS.COMMENT = new CellComment(((IDailyData)data).WEEK_ID, CellComment.Field.FORECAST, string.Empty);
                    else ((IDailyData)data).FORECASTS.COMMENT = forecastComments.Where(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID).FirstOrDefault();


                    var QC = this.QCS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault();
                    var QD = this.QDS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault();

                    ((IDailyData)data).QC = QC ?? QcQd.AddEmpty(this.QCS, ((IDailyData)data).WEEK_ID);
                    ((IDailyData)data).QD = QD ?? QcQd.AddEmpty(this.QDS, ((IDailyData)data).WEEK_ID);

                    ((IDailyData)data).HOLIDAYS = MainWindow.myHolidays.GetItemForDay(((IDailyData)data).REPORT_AS_OF_DATE);
                    list.Add((IDailyData)data);
                }
            }

            //if (list.Count > 0)
            //{
            this.AddDays(list);
            this.AddLYQS(tab.CUSTOMER_NUMBER, tab.SKU.SKUID, this.Min(x => x.REPORT_AS_OF_DATE), this.Max(x => x.REPORT_AS_OF_DATE));
            //}

        }

        void _AddExtraDays(int numberOfWeeksToAdd)
        {
            try
            {
                _GetDaysInWeek();
                var nextDay = this.LASTDAYWITHREALDATA;
                var numberOfDaysToExpand = (numberOfWeeksToAdd * 7) + (7 - this.NUMBEROFDAYINLASTWEEK);
                for (int i = 0; i < numberOfDaysToExpand; i++)
                {
                    nextDay = nextDay.AddDays(1);
                    var nextDayOfWeek = nextDay.DayOfWeek;
                    if (this.DAYSINWEEK.Any(x => x == nextDayOfWeek))
                    {
                        var shipTo = this.Select(x => x.SHIP_TO_TP).FirstOrDefault();
                        var itemUPC = this.Select(x => x.ITEM_UPC).FirstOrDefault();
                        var reportAsOf = nextDay;
                        var firstDay = reportAsOf.StartOfWeek(this.DAY_OF_WEEK);
                        var weekNum = firstDay.WeekNumber(_Date.DayOfWeek(shipTo));
                        var year = firstDay.Year;

                        var data = new DailyData(true, weekNum, year, shipTo, itemUPC);
                        ((IDailyData)data).IS_FUTURE = true;
                        ((IDailyData)data).REPORT_AS_OF_DATE = nextDay;
                        ((IDailyData)data).ALL_REPORT_AS_OF_DATE = new List<DateTime>() { ((IDailyData)data).REPORT_AS_OF_DATE };
                        ((IDailyData)data).FIRST_DAY_OF_WEEK = ((IDailyData)data).REPORT_AS_OF_DATE.StartOfWeek(this.DAY_OF_WEEK);
                        //((IDailyData)data).SHIP_TO_TP = this.Select(x => x.SHIP_TO_TP).FirstOrDefault();
                        //((IDailyData)data).ITEM_UPC = this.Select(x => x.ITEM_UPC).FirstOrDefault();
                        ((IDailyData)data).UOM = this.Select(x => x.UOM).FirstOrDefault();
                        //((IDailyData)data).WEEK_NUMBER = ((IDailyData)data).FIRST_DAY_OF_WEEK.WeekNumber(_Date.DayOfWeek(((IDailyData)data).SHIP_TO_TP));
                        //((IDailyData)data).YEAR = ((IDailyData)data).FIRST_DAY_OF_WEEK.Year;

                        var currentWeekComment = this.CELL_COMMENTS.Where(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID);
                        ((IDailyData)data).QA_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.QA).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QA);
                        ((IDailyData)data).QO_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.QO).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QO);
                        ((IDailyData)data).QS_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.QS).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QS);
                        ((IDailyData)data).QW_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.QW).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.QW);
                        ((IDailyData)data).FSI = this.FSIS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault() ?? Fsi.AddEmpty(this.FSIS, ((IDailyData)data).WEEK_ID);

                        ((IDailyData)data).LYQS = this.Where(x => x.REPORT_AS_OF_DATE == ((IDailyData)data).LAST_YEAR_DATE).Select(x => x.QS).FirstOrDefault();
                        ((IDailyData)data).LY_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.LY).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.LY);

                        ((IDailyData)data).ORDER_DELIVERY_COMMENT = currentWeekComment.Where(x => x.FIELD == CellComment.Field.ORDER).FirstOrDefault() ?? CellComment.AddEmpty(this.CELL_COMMENTS, ((IDailyData)data).WEEK_ID, CellComment.Field.ORDER);

                        var forecastComments = currentWeekComment.Where(x => x.FIELD == CellComment.Field.FORECAST);


                        ((IDailyData)data).LYQW = this.Where(x => x.REPORT_AS_OF_DATE == ((IDailyData)data).LAST_YEAR_DATE).Select(x => x.QW).FirstOrDefault();
                        ((IDailyData)data).PERFORMANCES = PerformanceCollection.New(this.PERFORMANCES.Where(x => x.START_DATE <= ((IDailyData)data).REPORT_AS_OF_DATE && x.END_DATE >= ((IDailyData)data).REPORT_AS_OF_DATE), ((IDailyData)data).WEEK_ID, ((IDailyData)data).REPORT_AS_OF_DATE);
                        ((IDailyData)data).COMMENT = this.COMMENTS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault() ?? Comment.AddEmpty(this.COMMENTS, ((IDailyData)data).WEEK_ID);

                        if (!this.FORECASTS.Any(x => x.WEEKID == (((IDailyData)data).WEEK_ID))) this.FORECASTS.Add(ForecastCollection.GetEmpty(new Forecast(((IDailyData)data).WEEK_ID)));
                        ((IDailyData)data).FORECASTS = this.FORECASTS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault();
                        if (!forecastComments.Any(x => x.WEEK_ID == (((IDailyData)data).WEEK_ID))) ((IDailyData)data).FORECASTS.COMMENT = new CellComment(((IDailyData)data).WEEK_ID, CellComment.Field.FORECAST, string.Empty);
                        else ((IDailyData)data).FORECASTS.COMMENT = forecastComments.Where(x => x.WEEK_ID == ((IDailyData)data).WEEK_ID).FirstOrDefault();

                        var QC = this.QCS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault();
                        var QD = this.QDS.Where(x => x.WEEKID == ((IDailyData)data).WEEK_ID).FirstOrDefault();

                        ((IDailyData)data).QC = QC ?? QcQd.AddEmpty(this.QCS, ((IDailyData)data).WEEK_ID);
                        ((IDailyData)data).QD = QD ?? QcQd.AddEmpty(this.QDS, ((IDailyData)data).WEEK_ID);


                        this.Add(data);
                        this.Move(this.Count() - 1, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManger.Insert(ex);
                throw new ApplicationException("Unable to add extra days to the daily data");
            }
        }

        bool AddLYQS(string custId, string sku, DateTime start, DateTime end)
        {
            List<datastores.VMI_Get_Customers_QS_Amounts_by_period_Result> list = null;
            try
            {
                using (var context = new datastores.dbidbEntities())
                {
                    try
                    {
                        list = context.VMI_Get_Customers_QS_Amounts_by_period(int.Parse(custId).ToString(), sku, start.LastYearDate(), end.LastYearDate()).ToList();
                    }
                    catch (AccessViolationException) { }
                }
                if (list != null && list.Count() > 0)
                    this.Update(item => item.LYQS = (list.Any(qs => qs.ReportAsOfDate == item.LAST_YEAR_DATE)) ? list.Where(qs => qs.ReportAsOfDate == item.LAST_YEAR_DATE).Sum(qs=>qs.Quantity) : 0);
                return true;
            }
            catch (Exception ex)
            {
                var msg = String.Format("AddLYQS for customer {0} and sku {1} for starting {2} and ending {3}", custId, sku, start, end);
                LogManger.Insert1(ex, msg);
                if (ex.InnerException != null)
                    LogManger.Insert1(ex.InnerException, msg);
            }
            return false;
        }

        void _GetDaysInWeek()
        {
            if (this.Count() > 0)
            {
                var a = this.GroupBy(x => x.WEEK_ID);
                var b = from g in a
                        select g.Count();
                var c = a.Where(g => g.Count() == b.Max()).FirstOrDefault();
                this.DAYSINWEEK = c.Select(x => x.REPORT_AS_OF_DATE.DayOfWeek);
                this.NUMBEROFDAYSINWEEK = b.Max();
                this.LASTDAYWITHREALDATA = this.Max(x => x.REPORT_AS_OF_DATE);
                this.NUMBEROFDAYINLASTWEEK = this.Where(x => x.WEEK_ID == this.Where(d => d.REPORT_AS_OF_DATE == this.LASTDAYWITHREALDATA).Select(w => w.WEEK_ID).FirstOrDefault()).Count();
            }
        }

        double _GetAverageTurn(int weeks)
        {
            //IEnumerable<IDailyData> coll;
            //if (this.SKUID.SKUID == "RC2420")
            //{
            //    coll = this.Where(x => x.REPORT_AS_OF_DATE <= this.LASTDAYOFLASTWEEKWITHREALDATA).Where(x => !x.HAS_HOLIDAY && !x.HAS_PERFORMANCES).OrderByDescending(x => x.REPORT_AS_OF_DATE).Take(weeks);
            //}else
            var coll = 
                this.Where(x => x.REPORT_AS_OF_DATE <= this.LASTDAYOFLASTWEEKWITHREALDATA)
                    .Where(x => !x.HAS_HOLIDAY && !x.HAS_PERFORMANCES)
                    .OrderByDescending(x => x.REPORT_AS_OF_DATE)
                    .Take(weeks);
            
            var qsSum = coll.Sum(x => x.QS);
            var average = coll.Count();
            return Convert.ToDouble(qsSum) / average;
        } 
        #endregion

    }
}
