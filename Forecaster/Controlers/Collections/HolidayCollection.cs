using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using daisybrand.forecaster.Controlers.Interfaces;
using daisybrand.forecaster.Helpers;
using datastores = CRDataStore;
namespace daisybrand.forecaster.Controlers.Collections
{
    public class HolidayCollection:ObservableCollection<Controlers.Interfaces.IHoldiay>, INotifyPropertyChanged, IDisposable
    {
        public HolidayCollection() { }
       


        public HolidayCollection(List<Controlers.Interfaces.IHoldiay> list)
            : base(list)
        {
            
            //if (changingEvent)
            //    this.CollectionChanged += HolidayCollection_CollectionChanged;
            //else
                //this.CollectionChanged += SubHolidayCollection_CollectionChanged;
        }
        public HolidayCollection(IEnumerable<Controlers.Interfaces.IHoldiay> collection)
            : base(collection)
        {
            
            this.CollectionChanged += HolidayCollection_CollectionChanged;
        }

        //public void AddRange(IEnumerable<Controlers.Interfaces.IHoldiay> collection)
        //{
        //    foreach (var item in collection)
        //        this.Add(item);
        //}

        void SubHolidayCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    var newItems = e.NewItems;
            //    var coll = MainWindow.myWeeklyData.Where(x => (x.ALL_DAYS_IN_WEEK.Max() >= hol.START && x.ALL_DAYS_IN_WEEK.Min() <= hol.START)
            //                        || (x.ALL_DAYS_IN_WEEK.Max() >= hol.END && x.ALL_DAYS_IN_WEEK.Min() <= hol.END)
            //                        || (x.ALL_DAYS_IN_WEEK.Max() <= hol.END && x.ALL_DAYS_IN_WEEK.Min() >= hol.START));
            //            foreach (var data in coll)  
            //                //data.HOLIDAYS.TOO
            //}
        }
        void HolidayCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (MainWindow.myMainWindowViewModel.TABS == null) return;
            foreach (var tab in MainWindow.myMainWindowViewModel.TABS)
            {
                if (tab.myDailyData != null)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        var newItems = e.NewItems;
                        foreach (var item in newItems)
                        {
                            var hol = (IHoldiay)item;

                            var coll = tab.myDailyData.Where(x => (x.REPORT_AS_OF_DATE >= hol.START && x.REPORT_AS_OF_DATE <= hol.END));
                            foreach (var data in coll)
                                data.HolidayAdding(hol); // = this.GetListForDays(data.ALL_DAYS_IN_WEEK);
                        }
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        var oldItems = e.OldItems;
                        foreach (var item in oldItems)
                        {
                            var hol = (IHoldiay)item;
                            var coll = tab.myDailyData.Where(x => (x.ALL_DAYS_IN_WEEK.Max() >= hol.START && x.ALL_DAYS_IN_WEEK.Min() <= hol.END && x.HOLIDAYS != null));
                            foreach (var data in coll)
                                data.HolidayRemoving(data.HOLIDAYS.Where(x => x.HOLIDAYID == hol.HOLIDAYID).FirstOrDefault());  //= this.GetListForDays(data.ALL_DAYS_IN_WEEK);
                        }
                    }
                }
                if (tab.myWeeklyData != null)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        var newItems = e.NewItems;
                        foreach (var item in newItems)
                        {
                            var hol = (IHoldiay)item;

                            var coll = tab.myWeeklyData.Where(x => (x.ALL_DAYS_IN_WEEK.Max() >= hol.START && x.ALL_DAYS_IN_WEEK.Min() <= hol.START)
                                        || (x.ALL_DAYS_IN_WEEK.Max() >= hol.END && x.ALL_DAYS_IN_WEEK.Min() <= hol.END)
                                        || (x.ALL_DAYS_IN_WEEK.Max() <= hol.END && x.ALL_DAYS_IN_WEEK.Min() >= hol.START));
                            foreach (var data in coll)
                                data.HolidayAdding(hol); // = this.GetListForDays(data.ALL_DAYS_IN_WEEK);
                        }
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        var oldItems = e.OldItems;
                        foreach (var item in oldItems)
                        {
                            var hol = (IHoldiay)item;
                            var coll = tab.myWeeklyData.Where(x => (x.ALL_DAYS_IN_WEEK.Max() >= hol.END && x.ALL_DAYS_IN_WEEK.Min() <= hol.END)
                                        || (x.ALL_DAYS_IN_WEEK.Max() >= hol.START && x.ALL_DAYS_IN_WEEK.Min() <= hol.START)
                                        || (x.ALL_DAYS_IN_WEEK.Max() <= hol.END && x.ALL_DAYS_IN_WEEK.Min() >= hol.START));
                            foreach (var data in coll)
                                data.HolidayRemoving(data.HOLIDAYS.Where(x => x.HOLIDAYID == hol.HOLIDAYID).FirstOrDefault());  //= this.GetListForDays(data.ALL_DAYS_IN_WEEK);
                        }
                    }
                }
                if (tab.myPercentageViewModel != null)
                    tab.myPercentageViewModel.Update();
                if (tab.myGraphViewModel != null)
                    tab.myGraphViewModel.SetQSStatisticalData();

            }
            
        }
        #region properties


        private bool _HAS_HOLIDAYS;
        private string _TOOLTIP;
        
        
        public bool HAS_HOLIDAYS
        {
            get
            {
                return _HAS_HOLIDAYS;
            }
            set
            {
                _HAS_HOLIDAYS = value;
                NotifyPropertyChanged();
            }
        }

        
        public string TOOLTIP
        {
            get
            {
                return _TOOLTIP;
            }
            set
            {
                _TOOLTIP = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        public static HolidayCollection Get()
        {
            try
            {
                List<datastores.FORECASTER_HOLIDAYS> list;
                using (var context = new datastores.ForecasterEntities(null))
                //new datastores.FORECASTER_HOLIDAYSTableAdapters.FORECASTER_HOLIDAYSTableAdapter())
                {
                    list = context.FORECASTER_HOLIDAYS.ToList();
                }
                if (list != null && list.Count() > 0)
                    return new HolidayCollection(
                                       list.Select(x => new Controlers.Objects.Holiday
                                        {
                                            DESCRIPTION = x.DESCRIPTION,
                                            END = x.END,
                                            START = x.START,
                                            HOLIDAYID = x.HOLIDAYID
                                        }));
            }
            catch (Exception ex)
            {
                LogManger.Insert1(ex, "Unable to get a list of Holidays");
            }
            return null;
        }



        public HolidayCollection GetItemForDay(DateTime date)
        {
            var coll = new List<IHoldiay>();
            coll.AddRange(this.Where(x => x.START <= date && x.END.AddDays(1) > date));
            return new HolidayCollection(coll);
        }

        public HolidayCollection GetListForDays(IEnumerable<DateTime> days)
        {
            var coll = new List<IHoldiay>();
            foreach (var date in days)
            {
                var r = coll.Select(x => x.HOLIDAYID);
                if (this.Where(x => r.Contains(x.HOLIDAYID)).Count() == 0)
                    coll.AddRange(this.Where(x => x.START <= date && x.END.AddDays(1) > date));
            }
            return new HolidayCollection(coll);
        }

        #region overrides
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var h in this)
                sb.Append(String.Format("{0} - {1}-{2}\r", h.DESCRIPTION, h.START.ToShortDateString(), h.END.ToShortDateString()));

            return sb.ToString();
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
            }

            // Free any unmanaged objects here. 
            //
            //this.CollectionChanged -= SubHolidayCollection_CollectionChanged;
            this.CollectionChanged -= HolidayCollection_CollectionChanged;
            disposed = true;
        }
        #endregion

    }
}
